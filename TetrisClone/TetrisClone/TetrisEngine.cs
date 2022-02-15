using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Oscetch.MonoGame.Extensions;
using Oscetch.MonoGame.Input.Managers;
using Oscetch.MonoGame.Input.Services;
using Oscetch.MonoGame.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using TetrisClone.Objects;
using TetrisClone.Utils;

namespace TetrisClone
{
    public class TetrisEngine
    {
        private static readonly Random _random = new Random();

        private readonly List<TetrisShapeBase> _activeShapes;
        private readonly KeyboardStateService _keyboard;
        private readonly Delay _gameDelay = new Delay(1000);
        private readonly QTree.DynamicQuadTree<TetrisBlock> _qTree = new QTree.DynamicQuadTree<TetrisBlock>();
        private readonly List<Func<TetrisShapeBase>> _shapeCreationList;

        private readonly int _rowCount;
        private readonly int _columnCount;

        private GraphicsDevice _graphicsDevice;

        private Texture2D _borderTexture;

        private TetrisShapeBase _currentControllableShape;

        private bool _shouldSpawn;
        private bool _shouldRestart;

        public int Score { get; set; }
        public Point GameSize { get; }
        public Dictionary<Point, Rectangle> GameBoard { get; }

        internal TetrisEngine(Point gameSize)
        {
            GameSize = gameSize;
            _rowCount = gameSize.Y / TetrisBlock.BLOCK_SIZE;
            _columnCount = gameSize.X / TetrisBlock.BLOCK_SIZE;
            GameBoard = gameSize.ToVector2()
                .ToRectangle(Vector2.Zero)
                .CreateGrid(_rowCount, _columnCount);
            _activeShapes = new List<TetrisShapeBase>();
            _keyboard = KeyboardManager.GetGeneral();

            _shapeCreationList = new List<Func<TetrisShapeBase>>
            {
                () => new LShape(GameSize),
                () => new IShape(GameSize),
                () => new BoxShape(GameSize),
                () => new ZShape(GameSize),
                () => new ArrowShape(GameSize)
            };
        }

        private void SpawnShape()
        {
            var shape = _shapeCreationList[_random.Next(_shapeCreationList.Count)]();
            shape.LoadContent(_graphicsDevice);
            _qTree.AddRange(shape.Blocks.ToArray());

            _activeShapes.Add(shape);

            _currentControllableShape = shape;
            _currentControllableShape.WasStopped += OnShape_WasStopped;
            _shouldSpawn = false;
        }

        private void OnShape_WasStopped(object sender, EventArgs e)
        {
            if(sender is not TetrisShapeBase shape)
            {
                return;
            }

            if(shape.Blocks.Min(x => x.Position.Y) < 0)
            {
                _shouldRestart = true;
            }

            _currentControllableShape = null;
            shape.WasStopped -= OnShape_WasStopped;
            _shouldSpawn = true;
        }

        public void Restart()
        {
            _gameDelay.DelayTime = 1000;
            _activeShapes.Clear();
            _qTree.Clear();
            _shouldRestart = false;
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            var parameters = new CustomTextureParameters.CustomTextureParametersBuilder()
                .WithFillColor(Color.White)
                .Build();
            _borderTexture = CustomTextureManager.GetCustomTexture(parameters, graphicsDevice);

            SpawnShape();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_borderTexture, new Rectangle(0, 0, 1, GameSize.Y), Color.White);
            spriteBatch.Draw(_borderTexture, new Rectangle(0, 0, GameSize.X, 1), Color.White);
            spriteBatch.Draw(_borderTexture, new Rectangle(GameSize.X - 1, 0, 1, GameSize.Y), Color.White);
            spriteBatch.Draw(_borderTexture, new Rectangle(0, GameSize.Y - 1, GameSize.X, 1), Color.White);

            foreach (var activeShapes in _activeShapes)
            {
                activeShapes.Draw(spriteBatch);
            }
        }

        private void RemoveRows()
        {
            for(var y = 0; y < _rowCount; y++)
            {
                var blocksOnRow = new List<TetrisBlock>();
                for(var x = 0; x < _columnCount; x++)
                {
                    var boardPosition = GameBoard[new Point(x, y)].Center;

                    var objects = _qTree.FindObject(boardPosition.X, boardPosition.Y);
                    if(objects.Count == 0 
                        || objects.Any(x => x.Shape.Id == (_currentControllableShape?.Id ?? -1)))
                    {
                        blocksOnRow.Clear();
                        break;
                    }

                    blocksOnRow.AddRange(objects);
                }
                if(blocksOnRow.Count > 0)
                {
                    _gameDelay.DelayTime *= .95; 
                }

                Score += blocksOnRow.Count;
                foreach(var blockOnRow in blocksOnRow)
                {
                    blockOnRow.Shape.RemoveBlock(blockOnRow);
                    if(blockOnRow.Shape.Blocks.Count == 0)
                    {
                        RemoveShape(blockOnRow.Shape);
                    }
                }
            }

            RefreshQTree();
        }

        private void RemoveShape(TetrisShapeBase tetrisShapeBase)
        {
            for(var i = 0; i < _activeShapes.Count; i++)
            {
                if(_activeShapes[i].Id == tetrisShapeBase.Id)
                {
                    _activeShapes.RemoveAt(i);
                    break;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            _gameDelay.Wait(gameTime, () =>
            {
                foreach(var activeShape in _activeShapes)
                {
                    activeShape.OnGameTick(_qTree);
                }

                RemoveRows();
            });
            if (_shouldSpawn)
            {
                SpawnShape();
            }
            if (_shouldRestart)
            {
                Restart();
            }

            if(_currentControllableShape == null)
            {
                return;
            }

            if (_keyboard.IsKeyClicked(Keys.R) 
                && _currentControllableShape.TryRotate(_qTree))
            {
                RefreshQTree();
            }
            if((_keyboard.IsKeyClicked(Keys.Left)
                || _keyboard.IsKeyClicked(Keys.A))
                && _currentControllableShape.TryMoveLeft(_qTree))
            {
                RefreshQTree();
            }
            if((_keyboard.IsKeyClicked(Keys.Right)
                || _keyboard.IsKeyClicked(Keys.D))
                 && _currentControllableShape.TryMoveRight(_qTree))
            {
                RefreshQTree();
            }
            if(_keyboard.IsKeyDown(Keys.S) || _keyboard.IsKeyDown(Keys.Down))
            {
                _currentControllableShape?.OnGameTick(_qTree);
                RefreshQTree();
            }
        }

        private void RefreshQTree()
        {
            _qTree.Clear();
            _qTree.AddRange(_activeShapes.SelectMany(x => x.Blocks).ToArray());
        }
    }
}
