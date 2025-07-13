using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.MonoGame.Camera;
using Oscetch.MonoGame.GuiComponent;
using Oscetch.MonoGame.GuiComponent.Interfaces;
using Oscetch.MonoGame.GuiComponent.Extensions;
using Oscetch.MonoGame.GuiComponent.Models;
using Oscetch.MonoGame.Input.Managers;
using System.Collections.Generic;

namespace TetrisClone
{
    public class GameScene : IGuiScene
    {
        private CameraHandler _guiCamera;
        private CameraHandler _cameraHandler;
        private TetrisEngine _tetrisEngine;

        public GuiCanvas<IGameToGuiService> Canvas { get; set; }

        public IGameToGuiService CreateGameToGuiService() => new GameToGuiService(_tetrisEngine);

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: _cameraHandler.ViewMatrix);
            _tetrisEngine.Draw(spriteBatch);
            spriteBatch.End();

            Canvas.Draw(spriteBatch, _guiCamera);
        }

        public void Initialize(ContentManager content, GraphicsDevice graphicsDevice, bool isEditor = false)
        {
            var gameSize = new Point(600, 720);
            _cameraHandler = new CameraHandler(1280, 720);
            _guiCamera = new CameraHandler(1280, 720);
            _cameraHandler.CenterCameraToTarget(gameSize.ToVector2() / 2);
            _tetrisEngine = new TetrisEngine(gameSize);
            _tetrisEngine.LoadContent(graphicsDevice);
            if (!isEditor)
            {
                Canvas = content.Load<List<GuiControlParameters>>("testcanvas.gui").ToCanvas(CreateGameToGuiService());
                Canvas.LoadContent(content, graphicsDevice);
            }
        }

        public void Update(GameTime gameTime)
        {
            KeyboardManager.Update();
            MouseManager.Update();
            Canvas.Update(gameTime, _guiCamera);
            _tetrisEngine.Update(gameTime);
        }
    }
}
