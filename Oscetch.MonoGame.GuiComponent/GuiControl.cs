using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.GuiComponent.Enums;
using Oscetch.MonoGame.Camera;
using Oscetch.MonoGame.Extensions;
using Oscetch.MonoGame.GuiComponent.Interfaces;
using Oscetch.MonoGame.GuiComponent.Models;
using Oscetch.MonoGame.GuiComponent.Services;
using Oscetch.MonoGame.Input.Managers;
using Oscetch.MonoGame.Math.Objects;
using Oscetch.ScriptComponent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Oscetch.MonoGame.GuiComponent
{
    public class GuiControl<T> where T : IGameToGuiService
    {
        private static ulong _idCounter = 0;
        private static Vector2 _borderLineOrigin = new(.5f);
        private readonly List<GuiControl<T>> _children;
        private readonly List<Line> _borderLines;

        private bool _translateToViewPort = true;
        private Func<Rectangle, bool> _isOverFunc = MouseManager.IsOverArea;

        public ulong Id { get; }

        public GuiControlParameters Parameters { get; }
        public IReadOnlyList<GuiControl<T>> Children => _children;
        public Texture2D Background { get; set; }
        public List<IGuiScript<T>> LoadedScripts { get; private set; }
        public string Name => Parameters.Name;
        public SpriteFont SpriteFont { get; set; }
        public string Text
        {
            get => Parameters.Text;
            set => Parameters.Text = value;
        }
        public Vector2 TextPosition
        {
            get => Parameters.TextPosition;
            set => Parameters.TextPosition = value;
        }
        public bool TranslateToViewPort
        {
            get => _translateToViewPort;
            set
            {
                if (value)
                {
                    _isOverFunc = MouseManager.IsOverArea;
                }
                else
                {
                    _isOverFunc = MouseManager.IsOverAreaRaw;
                }
                _translateToViewPort = value;
            }
        }
        public Vector2 Position
        {
            get => Parameters.Position;
            set 
            { 
                Parameters.Position = value;
                UpdateBorder();
            }
        }
        public Vector2 Size
        {
            get => Parameters.Size;
            set 
            { 
                Parameters.Size = value;
                UpdateBorder();
            }
        }
        public Color TextColor
        {
            get => Parameters.TextColor;
            set => Parameters.TextColor = value;
        }
        public float TextRotation
        {
            get => Parameters.TextRotation;
            set => Parameters.TextRotation = value;
        }
        public float TextScale
        {
            get => Parameters.TextScale;
            set => Parameters.TextScale = value;
        }
        public bool IsVisible
        {
            get => Parameters.IsVisible;
            set
            {
                if (value == Parameters.IsVisible)
                {
                    return;
                }

                Parameters.IsVisible = value;
                VisibleChanged(value);
            }
        }
        public bool IsEnabled
        {
            get => Parameters.IsEnabled;
            set => Parameters.IsEnabled = value;
        }

        public bool HasBorder
        {
            get => Parameters.HasBorder;
            set => Parameters.HasBorder = value;
        }

        public Color BorderColor
        {
            get => Parameters.BorderColor;
            set => Parameters.BorderColor = value;
        }
        public GuiControl<T> Parent { get; private set; }
        public Texture2D BorderTexture { get; set; }
        public virtual Rectangle Bounds => new(Position.ToPoint(), Size.ToPoint());
        public virtual Color Color { get; set; } = Color.White;
        public MouseOverControlState MouseOverGameObjectState { get; protected set; } = MouseOverControlState.Outside;

        protected readonly T GameToGuiService;
        public GuiControl(GuiControlParameters customControlParameters, T gameToGuiService)
        {
            Id = _idCounter;
            _idCounter++;
            _borderLines = [];
            Parameters = customControlParameters;

            _children = [];
            LoadedScripts = [];

            foreach (var child in Parameters.ChildControls)
            {
                var childControl = new GuiControl<T>(child, gameToGuiService)
                {
                    Parent = this
                };
                _children.Add(childControl);
            }

            GameToGuiService = gameToGuiService;
            Position = Parameters.Position;
            Size = Parameters.Size;
        }

        public void Unload()
        {
            foreach (var child in Children)
            {
                child.Unload();
            }

            BorderTexture?.Dispose();
            Background?.Dispose();
        }

        public void VisibleChanged(bool isVisible)
        {
            PerformScriptAction(x => x.VisibleChanged(isVisible));
            foreach (var child in Children)
            {
                child.VisibleChanged(isVisible);
            }
        }

        public void HideAndDisable()
        {
            IsEnabled = false;
            IsVisible = false;
        }

        public void ShowAndEnable()
        {
            IsVisible = true;
            IsEnabled = true;
        }

        public void AddChild(GuiControl<T> child)
        {
            if (_children.Any(x => x.Id == child.Id))
            {
                return;
            }

            child.Parent = this;
            _children.Add(child);
            Parameters.ChildControls.Add(child.Parameters);
        }

        public void RemoveChild(GuiControl<T> child)
        {
            for (var i = 0; i < _children.Count; i++)
            {
                if (child.Id != _children[i].Id)
                {
                    continue;
                }

                RemoveChild(i);
                return;
            }
        }

        public void RemoveChild(int index)
        {
            var child = _children[index];
            child.Parent = null;
            _children.RemoveAt(index);
            Parameters.ChildControls.RemoveAt(index);
        }

        public void InsertChild(GuiControl<T> child, int index)
        {
            child.Parent = this;
            _children.Insert(index, child);
            Parameters.ChildControls.Insert(index, child.Parameters);
        }

        public void ReplaceChild(GuiControl<T> child, int index)
        {
            child.Parent = this;
            _children[index] = child;
            Parameters.ChildControls[index] = child.Parameters;
        }

        public void SwitchChildren(int index1, int index2)
        {
            var child1 = _children[index1];
            var child2 = _children[index2];

            _children[index1] = child2;
            _children[index2] = child1;

            Parameters.ChildControls[index1] = child2.Parameters;
            Parameters.ChildControls[index2] = child1.Parameters;
        }

        /// <summary>
        /// Searches for the child by the control id and returns the index. If no match is found it returns -1
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public int IndexOf(GuiControl<T> child)
        {
            for (var i = 0; i < _children.Count; i++)
            {
                if (_children[i].Id == child.Id)
                {
                    return i;
                }
            }

            return -1;
        }

        public void ClearChildren()
        {
            while (_children.Count > 0)
            {
                _children[0].Parent = null;
                _children.RemoveAt(0);
                Parameters.ChildControls.RemoveAt(0);
            }
        }

        public bool IsPositionOverControl(Vector2 position)
        {
            if (!IsVisible)
            {
                return false;
            }

            if (Bounds.Contains(position))
            {
                return true;
            }

            foreach (var child in Children)
            {
                if (child.IsPositionOverControl(position))
                {
                    return true;
                }
            }

            return false;
        }

        private void OnMouseEnter()
        {
            PerformScriptAction(x => x.MouseEnter());
        }

        private void OnMouseExit()
        {
            PerformScriptAction(x => x.MouseExit());
        }
        private void OnMouseLeftClick()
        {
            if (DragAndDropService<T>.IsDraging)
            {
                return;
            }
            PerformScriptAction(x => x.LeftClick());
        }

        private void OnMouseRightClick()
        {
            PerformScriptAction(x => x.RightClick());
        }

        private void PerformScriptAction(Action<IGuiScript<T>> action)
        {
            foreach (var script in LoadedScripts)
            {
                action.Invoke(script);
            }
        }

        private void LoadBackground(ContentManager contentManager, GraphicsDevice graphicsDevice, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (!Path.HasExtension(path))
            {
                Background = contentManager.Load<Texture2D>(path);
                return;
            }
            if (!File.Exists(path))
            {
                return;
            }

            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            Background = Texture2D.FromStream(graphicsDevice, fileStream);
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice, Vector2 resolution)
        {
            LoadBackground(contentManager, graphicsDevice, Parameters.BackgroundTexture2DPath);

            if (GameToGuiService != null)
            {
                foreach (var scriptReference in Parameters.BuiltInScripts)
                {
                    if (ScriptLoader.TryLoadBuiltInScriptReference<IGuiScript<T>>(scriptReference, out var script))
                    {
                        LoadedScripts.Add(script);
                    }
                }
                foreach (var scriptReference in Parameters.ControlScripts)
                {
                    if (ScriptLoader.TryLoadScriptReference<IGuiScript<T>>(scriptReference, out var script))
                    {
                        LoadedScripts.Add(script);
                    }
                }
            }

            BorderTexture = new Texture2D(graphicsDevice, 1, 1);
            BorderTexture.SetData(new[] { Color.White });

            Parameters.UpdateResolution(resolution);

            SpriteFont = contentManager.Load<SpriteFont>(Parameters.SpriteFont);

            foreach (var child in Children)
            {
                child.LoadContent(contentManager, graphicsDevice, resolution);
            }

            if (GameToGuiService == null)
            {
                return;
            }

            foreach (var script in LoadedScripts)
            {
                script.Initialize(this, contentManager, graphicsDevice, GameToGuiService);
            }
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            if (!string.IsNullOrEmpty(Parameters.BackgroundTexture2DPath))
            {
                Background = contentManager.Load<Texture2D>(Parameters.BackgroundTexture2DPath);
            }

            if (GameToGuiService != null)
            {
                foreach (var scriptReference in Parameters.BuiltInScripts)
                {
                    if (ScriptLoader.TryLoadBuiltInScriptReference<IGuiScript<T>>(scriptReference, out var script))
                    {
                        LoadedScripts.Add(script);
                    }
                }
                foreach (var scriptReference in Parameters.ControlScripts)
                {
                    if (ScriptLoader.TryLoadScriptReference<IGuiScript<T>>(scriptReference, out var script))
                    {
                        LoadedScripts.Add(script);
                    }
                }
            }

            BorderTexture = new Texture2D(graphicsDevice, 1, 1);
            BorderTexture.SetData(new[] { Color.White });

            Parameters.UpdateResolution(graphicsDevice.Viewport.Bounds.Size.ToVector2());

            SpriteFont = contentManager.Load<SpriteFont>(Parameters.SpriteFont);

            foreach (var child in Children)
            {
                child.LoadContent(contentManager, graphicsDevice);
            }

            if (GameToGuiService == null)
            {
                return;
            }

            foreach (var script in LoadedScripts)
            {
                script.Initialize(this, contentManager, graphicsDevice, GameToGuiService);
            }
        }

        public void Draw(SpriteBatch spriteBatch, CameraHandler cameraHandler)
        {
            if (!IsVisible)
            {
                return;
            }

            if (Background != null)
            {
                spriteBatch.Draw(Background, Bounds, Color.White);
            }

            if (!string.IsNullOrEmpty(Text))
            {
                if (Parameters.CenterText)
                {
                    TextPosition = Bounds.Center.ToVector2();
                }

                spriteBatch.DrawString(SpriteFont, Text, TextPosition, TextColor,
                    TextRotation, SpriteFont.MeasureString(Text) / 2, TextScale, SpriteEffects.None, 0);
            }

            if (HasBorder && _borderLines.Count > 0 && BorderTexture != null)
            {
                foreach (var line in _borderLines)
                {
                    DrawLine(line, spriteBatch, cameraHandler);
                }
            }

            foreach (var child in Children)
            {
                child.Draw(spriteBatch, cameraHandler);
            }
        }

        private void DrawLine(Line line, SpriteBatch spriteBatch, CameraHandler cameraHandler)
        {
            spriteBatch.Draw(BorderTexture, line.MidPoint, null, BorderColor, line.GetAngleInRadians(), _borderLineOrigin,
                    new Vector2(line.Length(), 1 / cameraHandler.Scale),
                    SpriteEffects.None, 0);
        }

        private void UpdateMouseOver()
        {
            switch (MouseOverGameObjectState)
            {
                case MouseOverControlState.Enter:
                    MouseOverGameObjectState = MouseOverControlState.Over;
                    break;
                case MouseOverControlState.Exit:
                    MouseOverGameObjectState = MouseOverControlState.Outside;
                    break;
                case MouseOverControlState.Outside:
                    if (_isOverFunc(new Rectangle(Position.ToPoint(), Size.ToPoint())))
                    {
                        MouseOverGameObjectState = MouseOverControlState.Enter;
                        OnMouseEnter();
                    }
                    break;
                case MouseOverControlState.Over:
                    if (!_isOverFunc(new Rectangle(Position.ToPoint(), Size.ToPoint())))
                    {
                        MouseOverGameObjectState = MouseOverControlState.Exit;
                        OnMouseExit();
                    }
                    else if (MouseManager.IsLeftButtonClicked)
                    {
                        OnMouseLeftClick();
                    }
                    else if (MouseManager.IsRightButtonClicked)
                    {
                        OnMouseRightClick();
                    }
                    break;
            }
        }

        private void UpdateBorder()
        {
            if (HasBorder)
            {
                _borderLines.Clear();
                _borderLines.AddRange(Bounds.ToLines());
            }
        }

        public void Update(GameTime gameTime, CameraHandler cameraHandler)
        {
            if (!IsEnabled)
            {
                return;
            }
            UpdateMouseOver();

            for (var i = 0; i < _children.Count; i++)
            {
                _children[i].Update(gameTime, cameraHandler);
            }

            foreach (var script in LoadedScripts)
            {
                script.Update(gameTime, cameraHandler);
            }
        }

        public void MoveControlAndChildren(Vector2 newPosition)
        {
            var positionDiff = newPosition - Position;

            Position = newPosition;
            foreach (var control in _children)
            {
                control.Position += positionDiff;
                control.TextPosition += positionDiff;
            }
        }

        public void PlaceAroundBounds(CameraHandler cameraHandler, Rectangle worldBounds, int minXPosition)
        {
            var positionOnScreen = cameraHandler.WorldToScreen(worldBounds.Location.ToVector2());
            var sizeOnScreen = worldBounds.Size.ToVector2() * cameraHandler.Scale;
            var selectedBounds = new Rectangle(positionOnScreen.ToPoint(), sizeOnScreen.ToPoint());

            PlaceAroundBounds(new Rectangle(minXPosition, 0, cameraHandler.ScreenWidth - minXPosition, cameraHandler.ScreenHeight), selectedBounds);
        }

        public void PlaceAroundBounds(Rectangle outerBounds, Rectangle selectedBounds)
        {
            Vector2 newPosition;
            if (selectedBounds.Center.X < outerBounds.Center.X)
            {
                newPosition = new Vector2(System.Math.Max(selectedBounds.Right, outerBounds.X),
                    System.Math.Max(selectedBounds.Y, outerBounds.Y));
            }
            else
            {
                newPosition = new Vector2(
                    System.Math.Min(selectedBounds.X - Size.X, outerBounds.Right - Size.X),
                    System.Math.Max(selectedBounds.Y, outerBounds.Y));
            }

            var heightDiff = newPosition.Y + Size.Y - outerBounds.Bottom;

            if (heightDiff > 0)
            {
                newPosition -= new Vector2(0, heightDiff);
            }

            MoveControlAndChildren(newPosition);
        }
    }
}
