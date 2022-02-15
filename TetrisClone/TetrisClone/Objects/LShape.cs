using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.MonoGame.Extensions;
using System;
using System.Collections.Generic;

namespace TetrisClone.Objects
{
    public class LShape : TetrisShapeBase
    {
        private readonly List<Rectangle> _blockBounds;
        private int _width;
        private int _height;

        public LShape(Point gameSize) : base(gameSize)
        {
            _height = TetrisBlock.BLOCK_SIZE * 3;
            _width = TetrisBlock.BLOCK_SIZE * 2;
            var maxWidth = (gameSize.X / TetrisBlock.BLOCK_SIZE) - 1;
            var xPos = Random.Next(maxWidth) * TetrisBlock.BLOCK_SIZE;
            var position = new Vector2(xPos, -_height);
            var grid = new Vector2(_width, _height)
                .ToRectangle(position)
                .CreateGrid(3, 2);
            _blockBounds = new List<Rectangle>
            {
                grid[Point.Zero],
                grid[new Point(0, 1)],
                grid[new Point(0, 2)],
                grid[new Point(1, 2)]
            };
        }

        protected override List<Rectangle> Create180DegreeBounds()
        {
            var grid = new Vector2(_width, _height)
                .ToRectangle(Vector2.Zero)
                .CreateGrid(3, 2);

            return new List<Rectangle>
            {
                grid[Point.Zero],
                grid[new Point(1, 0)],
                grid[new Point(1, 1)],
                grid[new Point(1, 2)]
            };
        }

        protected override List<Rectangle> Create270DegreeBounds()
        {
            var grid = new Vector2(_height, _width)
                .ToRectangle(Vector2.Zero)
                .CreateGrid(2, 3);

            return new List<Rectangle>
            {
                grid[new Point(2, 0)],
                grid[new Point(2, 1)],
                grid[new Point(1, 1)],
                grid[new Point(0, 1)]
            };
        }

        protected override List<Rectangle> Create90DegreeBounds()
        {
            var grid = new Vector2(_height, _width)
                .ToRectangle(Vector2.Zero)
                .CreateGrid(2, 3);

            return new List<Rectangle>
            {
                grid[Point.Zero],
                grid[new Point(0, 1)],
                grid[new Point(1, 0)],
                grid[new Point(2, 0)],
            };
        }

        protected override List<Rectangle> CreateZeroBounds()
        {
            var grid = new Vector2(_width, _height)
                .ToRectangle(Vector2.Zero)
                .CreateGrid(3, 2);

            return new List<Rectangle>
            {
                grid[Point.Zero],
                grid[new Point(0, 1)],
                grid[new Point(0, 2)],
                grid[new Point(1, 2)]
            };
        }

        protected override List<TetrisBlock> CreateBlocks(GraphicsDevice graphicsDevice)
        {
            var tetrisBlocks = new List<TetrisBlock>();
            foreach(var bounds in _blockBounds)
            {
                var block = new TetrisBlock(this)
                {
                    Position = bounds.Center.ToVector2()
                };
                block.LoadContent(graphicsDevice);
                block.Color = Color.Orange;

                tetrisBlocks.Add(block);
            }
            return tetrisBlocks;
        }
    }
}
