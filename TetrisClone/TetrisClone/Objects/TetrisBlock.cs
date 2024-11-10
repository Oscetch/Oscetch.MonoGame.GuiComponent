using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.MonoGame.Textures;
using QTree;
using QTree.Interfaces;

namespace TetrisClone.Objects
{
    public class TetrisBlock : IQuadTreeObject<TetrisBlock>
    {
        public const int BLOCK_SIZE = 40;

        private Texture2D _texture;

        public Color Color { get; set; } = Color.White;
        public Vector2 Position { get; set; }
        public Vector2 Size { get; } = new Vector2(BLOCK_SIZE);

        public QuadId Id { get; } = new QuadId();

        public TetrisShapeBase Shape { get; }

        public QTree.Util.Rectangle Bounds => new QTree.Util.Rectangle(
            (int)(Position.X - (Size.X / 2)), 
            (int)(Position.Y - (Size.Y / 2)), 
            (int)Size.X, (int)Size.Y);

        public TetrisBlock Object => this;

        public TetrisBlock(TetrisShapeBase shape)
        {
            Shape = shape;
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            var customTextureParameter = new CustomTextureParameters.CustomTextureParametersBuilder()
                .WithFillColor(Color.White)
                .WithBorderColor(Color.Black)
                .WithBorderThickness(1)
                .WithSize(Size)
                .Build();
            _texture = CustomTextureManager.GetCustomTexture(customTextureParameter, graphicsDevice);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color, 0f, Size / 2, Vector2.One, SpriteEffects.None, 0);
        }
    }
}
