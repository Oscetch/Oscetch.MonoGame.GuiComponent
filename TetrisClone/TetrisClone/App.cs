using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.MonoGame.Camera;
using Oscetch.MonoGame.GuiComponent;
using Oscetch.MonoGame.Input.Managers;

namespace TetrisClone
{
    public class App : Game
	{
		private readonly GraphicsDeviceManager _graphics;
		private readonly GameToGuiService _gameToGuiService;
        private readonly GuiCanvas<GameToGuiService> _guiCanvas;
		private readonly CameraHandler _cameraHandler;
		private readonly CameraHandler _guiCamera;
		private readonly TetrisEngine _tetrisEngine;
		private SpriteBatch _spriteBatch;

		public App()
		{
			Content.RootDirectory = "Content";
			var gameSize = new Point(600, 720);
			_cameraHandler = new CameraHandler(1280, 720);
			_guiCamera = new CameraHandler(1280, 720);
			_cameraHandler.CenterCameraToTarget(gameSize.ToVector2() / 2);
			_tetrisEngine = new TetrisEngine(gameSize);
			_gameToGuiService = new GameToGuiService(_tetrisEngine);
			_guiCanvas = new GuiCanvas<GameToGuiService>("testcanvas.json", _gameToGuiService);
			_graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 1280,
				PreferredBackBufferHeight = 720,
			};
			IsMouseVisible = true;
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_guiCanvas.LoadContent(Content, GraphicsDevice);
			_tetrisEngine.LoadContent(GraphicsDevice);

			base.LoadContent();
		}

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

			KeyboardManager.Update();
			_guiCanvas.Update(gameTime, _guiCamera);
			_tetrisEngine.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			_spriteBatch.Begin(transformMatrix: _cameraHandler.ViewMatrix);
			_tetrisEngine.Draw(_spriteBatch);
			_spriteBatch.End();

			_guiCanvas.Draw(_spriteBatch, _guiCamera);
		}
	}
}
