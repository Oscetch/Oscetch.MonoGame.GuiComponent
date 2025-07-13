using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisClone
{
    public class App : Game
	{
		private readonly GameScene _scene = new ();
		private SpriteBatch _spriteBatch;

		public App()
		{
			Content.RootDirectory = "Content";
			_ = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 1280,
				PreferredBackBufferHeight = 720,
			};
			IsMouseVisible = true;
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_scene.Initialize(Content, GraphicsDevice);

			base.LoadContent();
		}

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

			_scene.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			_scene.Draw(gameTime, _spriteBatch);
		}
	}
}
