using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.MonoGame.GuiComponent.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Oscetch.MonoGame.GuiComponent.Models;
using System.Diagnostics;
using Oscetch.MonoGame.GuiComponent.Services;
using Oscetch.MonoGame.Camera;
using Oscetch.MonoGame.Input.Managers;

namespace Oscetch.MonoGame.GuiComponent
{
    public class GuiCanvas<T> where T : IGameToGuiService
    {
        private readonly T _gameToGuiService;
        private readonly Func<List<GuiControlParameters>> _loadingFunction;
        private readonly DragAndDropService<T> _dragAndDropService;

        private GuiControl<T> _activeModal;

        public List<GuiControl<T>> Controls { get; } = [];
        public bool IsMouseOverControl { get; private set; }

        public GuiCanvas(string parametersFilePath, T gameToGuiService) : this(() =>
        {
            return File.Exists(parametersFilePath)
                ? JsonConvert.DeserializeObject<List<GuiControlParameters>>(File.ReadAllText(parametersFilePath))
                : [];
        }, gameToGuiService) { }

        public GuiCanvas(Func<List<GuiControlParameters>> loadingFunction, T gameToGuiService)
        {
            _gameToGuiService = gameToGuiService;
            _loadingFunction = loadingFunction;
            _dragAndDropService = new DragAndDropService<T>(this);
        }

        public bool IsPositionOverControl(Vector2 position)
        {
            foreach (var control in Controls)
            {
                if (control.IsPositionOverControl(position))
                {
                    return true;
                }
            }

            return false;
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            try
            {
                GuiControl<T>.GraphicsDevice = graphicsDevice;
                var controlParameters = _loadingFunction();

                var nonVisibleIndexesQueue = new Queue<int>();
                for (var i = 0; i < controlParameters.Count; i++)
                {
                    var parameters = controlParameters[i];
                    var control = new GuiControl<T>(parameters, _gameToGuiService);

                    if (!control.IsVisible)
                    {
                        nonVisibleIndexesQueue.Enqueue(i);
                    }
                    else if (nonVisibleIndexesQueue.Count != 0)
                    {
                        var insertIndex = nonVisibleIndexesQueue.Dequeue();
                        var previousControlAtIndex = Controls[insertIndex];
                        Controls[insertIndex] = control;
                        Controls.Add(previousControlAtIndex);
                        continue;
                    }

                    Controls.Add(control);
                }

                foreach (var control in Controls)
                {
                    control.LoadContent(contentManager, graphicsDevice);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void UnloadContent()
        {
            foreach(var control in Controls)
            {
                control.Unload();
            }
        }

        private void SwitchModal(GuiControl<T> newModal)
        {
            _activeModal?.HideAndDisable();
            _activeModal = newModal;
            _activeModal.ShowAndEnable();
        }

        public void Draw(SpriteBatch spriteBatch, CameraHandler cameraHandler)
        {
            spriteBatch.Begin(transformMatrix: cameraHandler.ViewMatrix);

            foreach (var control in Controls)
            {
                control.Draw(spriteBatch, cameraHandler);
            }
            _dragAndDropService.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime, CameraHandler cameraHandler)
        {
            IsMouseOverControl = IsPositionOverControl(MouseManager.RawPosition.ToVector2());
            var nonVisibleIndexesQueue = new Queue<int>();

            for (var i = 0; i < Controls.Count; i++)
            {
                var control = Controls[i];
                if (!control.IsVisible)
                {
                    nonVisibleIndexesQueue.Enqueue(i);
                }
                else if (nonVisibleIndexesQueue.Count != 0)
                {
                    var replaceIndex = nonVisibleIndexesQueue.Dequeue();
                    var replaceControl = Controls[replaceIndex];
                    Controls[replaceIndex] = control;
                    Controls[i] = replaceControl;
                }

                if (control.Parameters.IsModal && control.IsEnabled)
                {
                    if (_activeModal == null || _activeModal.Id != control.Id)
                    {
                        SwitchModal(control);
                    }
                }

                control.Update(gameTime, cameraHandler);
            }

            _dragAndDropService.Update();
        }
    }
}
