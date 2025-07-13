using Editor.MonoGameControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.MonoGame.GuiComponent;
using Oscetch.MonoGame.GuiComponent.Interfaces;
using Oscetch.MonoGame.GuiComponent.Models;
using System;
using System.Collections.Generic;

namespace Editor.ViewModels
{
    internal class TestLayoutWindowViewModel(dynamic scene, List<GuiControlParameters> parameters) : MonoGameViewModel
    {
        private readonly dynamic _scene = scene;
        private Rectangle CurrentBounds;
        private SpriteBatch _spriteBatch;

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _scene.Initialize(Content, GraphicsDevice, true);
            Func<List<GuiControlParameters>> parametersFunc = () => parameters;
            var canvas = new GuiCanvas<IGameToGuiService>(parametersFunc, _scene.CreateGameToGuiService());
            CurrentBounds = GraphicsDevice.Viewport.Bounds;
            canvas.LoadContent(Content, GraphicsDevice);
            _scene.Canvas = canvas;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentBounds != GraphicsDevice.Viewport.Bounds)
            {
                CurrentBounds = GraphicsDevice.Viewport.Bounds;
                Func<List<GuiControlParameters>> parametersFunc = () => parameters;
                var canvas = new GuiCanvas<IGameToGuiService>(parametersFunc, _scene.CreateGameToGuiService());
                canvas.LoadContent(Content, GraphicsDevice);
                _scene.Canvas = canvas;
            }
            _scene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _scene.Draw(gameTime, _spriteBatch);
        }
    }
}
