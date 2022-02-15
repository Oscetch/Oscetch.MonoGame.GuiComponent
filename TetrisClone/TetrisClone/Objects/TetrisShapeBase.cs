using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QTree;
using QTree.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisClone.Enums;

namespace TetrisClone.Objects
{
    public abstract class TetrisShapeBase
    {
        protected static Random Random { get; } = new Random();

        private readonly Rectangle _gameBoard;

        private Dictionary<Rotation, List<Rectangle>> _rotationPositions 
            = new Dictionary<Rotation, List<Rectangle>>();

        private List<TetrisBlock> _blocks;
        private Rotation _currentRotation = Rotation.Zero;
        private bool _isDetached;

        public IReadOnlyList<TetrisBlock> Blocks => _blocks;

        private static long _idCounter = 0;
        public long Id { get; }

        public TetrisShapeBase(Point gameSize)
        {
            Id = _idCounter;
            _idCounter++;
            _gameBoard = new Rectangle(Point.Zero, gameSize);
        }

        public void RemoveBlock(TetrisBlock block)
        {
            for(var i = 0; i < _blocks.Count; i++)
            {
                if(_blocks[i].Id.Id == block.Id.Id)
                {
                    _blocks.RemoveAt(i);
                    _isDetached = true;
                    return;
                }
            }
        }

        protected abstract List<TetrisBlock> CreateBlocks(GraphicsDevice graphicsDevice);

        protected abstract List<Rectangle> CreateZeroBounds();
        protected abstract List<Rectangle> Create90DegreeBounds();
        protected abstract List<Rectangle> Create180DegreeBounds();
        protected abstract List<Rectangle> Create270DegreeBounds();

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            _blocks = CreateBlocks(graphicsDevice);
            _rotationPositions[Rotation.Zero] = CreateZeroBounds();
            _rotationPositions[Rotation.Ninety] = Create90DegreeBounds();
            _rotationPositions[Rotation.OneHundredEighty] = Create180DegreeBounds();
            _rotationPositions[Rotation.TwoHundredSeventy] = Create270DegreeBounds();
        }

        public bool TryMoveLeft(IQuadTree<TetrisBlock> quadTree)
        {
            var nextPositions = new List<Vector2>();
            foreach(var block in _blocks)
            {
                var nextPosition = block.Position - new Vector2(TetrisBlock.BLOCK_SIZE, 0);
                if (nextPosition.X < 0)
                {
                    return false;
                }
                if(QueryQTree(quadTree, nextPosition).Any())
                {
                    return false;
                }
                nextPositions.Add(nextPosition);
            }

            for(var i = 0; i < nextPositions.Count; i++)
            {
                _blocks[i].Position = nextPositions[i];
            }

            return true;
        }

        public bool TryMoveRight(IQuadTree<TetrisBlock> quadTree)
        {
            var nextPositions = new List<Vector2>();
            foreach (var block in _blocks)
            {
                var nextPosition = block.Position + new Vector2(TetrisBlock.BLOCK_SIZE, 0);
                if (nextPosition.X > _gameBoard.Width)
                {
                    return false;
                }
                if (QueryQTree(quadTree, nextPosition).Any())
                {
                    return false;
                }
                nextPositions.Add(nextPosition);
            }

            for (var i = 0; i < nextPositions.Count; i++)
            {
                _blocks[i].Position = nextPositions[i];
            }

            return true;
        }

        public bool TryRotate(IQuadTree<TetrisBlock> quadTree)
        {
            switch (_currentRotation)
            {
                case Rotation.Zero:
                    return MatchBlocksToPositions(Rotation.Ninety, quadTree);
                case Rotation.Ninety:
                    return MatchBlocksToPositions(Rotation.OneHundredEighty, quadTree);
                case Rotation.OneHundredEighty:
                    return MatchBlocksToPositions(Rotation.TwoHundredSeventy, quadTree);
                case Rotation.TwoHundredSeventy:
                    return MatchBlocksToPositions(Rotation.Zero, quadTree);
            }
            return false;
        }

        private bool MatchBlocksToPositions(Rotation rotation, IQuadTree<TetrisBlock> quadTree)
        {
            var rectangles = _rotationPositions[rotation];
            var xPosition = _blocks.Min(x => x.Position.X);
            var yPosition = _blocks.Min(x => x.Position.Y);

            var nextPositions = new List<Vector2>();
            foreach(var rectangle in rectangles)
            {
                var nextPosition = new Vector2(xPosition, yPosition) + rectangle.Location.ToVector2();
                if (!_gameBoard.Contains(nextPosition))
                {
                    return false;
                }

                if (QueryQTree(quadTree, nextPosition).Any())
                {
                    return false;
                }
                nextPositions.Add(nextPosition);
            }

            for(var i = 0; i < nextPositions.Count; i++)
            {
                _blocks[i].Position = nextPositions[i];
            }

            _currentRotation = rotation;

            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var block in _blocks)
            {
                block.Draw(spriteBatch);
            }
        }

        public void OnGameTick(IQuadTree<TetrisBlock> qTree)
        {
            var nextPositions = new List<Vector2>();
            foreach(var block in _blocks)
            {
                nextPositions.Add(block.Position + new Vector2(0, TetrisBlock.BLOCK_SIZE));
            }

            if (_isDetached)
            {
                OnDetachedGameTick(qTree, nextPositions);
            }
            else
            {
                OnAttachedGameTick(qTree, nextPositions);
            }
        }

        private void OnDetachedGameTick(IQuadTree<TetrisBlock> qTree, List<Vector2> nextPositions)
        {
            for (var i = 0; i < _blocks.Count; i++)
            {
                var nextPosition = nextPositions[i];
                if (nextPosition.Y >= _gameBoard.Height)
                {
                    continue;
                }

                if (qTree.FindNode((int)nextPosition.X, (int)nextPosition.Y).Any())
                {
                    continue;
                }

                _blocks[i].Position = nextPosition;
            }
        }

        private void OnAttachedGameTick(IQuadTree<TetrisBlock> qTree, List<Vector2> nextPositions)
        {
            foreach (var nextPosition in nextPositions)
            {
                if (nextPosition.Y >= _gameBoard.Height)
                {
                    WasStopped?.Invoke(this, EventArgs.Empty);
                    return;
                }

                if (QueryQTree(qTree, nextPosition).Any())
                {
                    WasStopped?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }

            for (var i = 0; i < _blocks.Count; i++)
            {
                _blocks[i].Position = nextPositions[i];
            }
        }

        private IEnumerable<IQuadTreeObject<TetrisBlock>> QueryQTree(IQuadTree<TetrisBlock> quadTree, Vector2 position)
        {
            return quadTree.FindNode((int)position.X, (int)position.Y)
                    .Where(x => !_blocks.Any(b => b.Id.Id == x.Id.Id));
        }

        public event EventHandler WasStopped;
    }
}
