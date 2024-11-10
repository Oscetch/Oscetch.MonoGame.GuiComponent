using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Editor.MonoGameControls;
using Editor.Enums;
using System.Collections.Generic;
using Oscetch.MonoGame.GuiComponent;
using Editor.Services;
using Oscetch.MonoGame.Camera;
using Oscetch.MonoGame.GuiComponent.Models;
using Editor.Models;
using System.Linq;
using System.Diagnostics;
using Oscetch.MonoGame.Extensions;
using Oscetch.MonoGame.Textures;
using Oscetch.MonoGame.Textures.Enums;
using System.Windows.Input;
using System.Windows;
using Point = Microsoft.Xna.Framework.Point;
using Oscetch.MonoGame.GuiComponent.Interfaces;

namespace Editor.ViewModels
{
    public class EditorViewModel : MonoGameViewModel
    {
        private readonly List<Rectangle> _alignmentIndicators = [];

        private DateTime _lastSelectedControlClick = DateTime.MinValue;

        private Vector2 _mousePosition = Vector2.Zero;
        private Vector2 _mouseDragPositionStart = Vector2.Zero;
        private Vector2 _controlDragPositionStart = Vector2.Zero;
        private Vector2 _controlDragSizeStart = Vector2.Zero;

        private Point _lastCameraDragMousePosition = Point.Zero;

        private Rectangle _leftBorderRect;
        private Rectangle _rightBorderRect;
        private Rectangle _topBorderRect;
        private Rectangle _bottomBorderRect;

        private Rectangle _drawableBoundsRect;

        private SpriteBatch _spriteBatch;
        private readonly WpfKeyboardService _keyboard = new();

        private Texture2D _invisibleControlBorderTexture;
        private Texture2D _alignmentTexture;
        private Texture2D _selectedBorderTexture;
        private Texture2D _borderMouseIndicator;
        private Texture2D _boundsIndicator;

        private SpriteFont _font;

        private GuiControl<IGameToGuiService> _customControl;

        private CameraHandler _cameraHandler;
        private DragMode _currentDragMode = DragMode.None;
        private SizeAnchor _currentSizeAnchor = SizeAnchor.Top;

        private bool _showIndicator;
        private bool _isDraggingCamera;

        private GuiControl<IGameToGuiService> _selectedControl;
        private GuiControl<IGameToGuiService> SelectedControl 
        { 
            get => _selectedControl; 
            set
            {
                _selectedControl = value;
                SelectedControlChanged?.Invoke(this, EventArgs.Empty);
            } 
        }

        private GuiControlParameters _copyParameters;

        public bool IsInitialized { get; private set; }

        public Vector2 Resolution { get; private set; }

        public Vector2 CenterOfScreen => _cameraHandler.Center;

        public GuiControlParameters Parameters => _customControl?.Parameters;

        public GuiControlParameters SelectedParameters => SelectedControl?.Parameters;

        public IReadOnlyList<GuiControl<IGameToGuiService>> Children => _customControl.Children;

        public ControlBuilderConfiguration Configuration { get; private set; } = new ControlBuilderConfiguration();

        public bool IsRootControlSelected => SelectedControl != null && SelectedControl.Id == _customControl.Id;

        private void SetDrawableArea()
        {
            _drawableBoundsRect = new Rectangle(0, 0, 1280, 720);
            Resolution = _drawableBoundsRect.Size.ToVector2();
            
            _cameraHandler = new CameraHandler(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)
            {
                CameraPosition = _cameraHandler.CameraPosition,
                Scale = _cameraHandler.Scale
            };
        }

        private void UpdateIndicator()
        {
            _showIndicator = GetSizeAnchor(_mousePosition) != SizeAnchor.None;
        }

        private static void UpdateControlPosition(GuiControl<IGameToGuiService> control, Vector2 positionDiff)
        {
            control.Position -= positionDiff;
            control.TextPosition -= positionDiff;
            foreach (var child in control.Children)
            {
                UpdateControlPosition(child, positionDiff);
            }
        }

        private bool TryGetControl(ulong controlId, out UiControlSearchResult searchResult)
        {
            if (controlId == _customControl.Id)
            {
                var canvasControl = new GuiControl<IGameToGuiService>(new GuiControlParameters(Resolution)
                {
                    ChildControls = [Parameters]
                }, null);

                searchResult = new UiControlSearchResult(canvasControl, 0);
                return true;
            }

            for (var i = 0; i < Children.Count; i++)
            {
                var result = new UiControlSearchResult(_customControl, i);

                if (!TryGetControl(result, controlId, out var newResult))
                {
                    continue;
                }

                searchResult = newResult;
                return true;
            }

            searchResult = null;
            return false;
        }

        private static bool TryGetControl(UiControlSearchResult currentResult, ulong controlId, out UiControlSearchResult searchResult)
        {
            if (currentResult.Control.Id == controlId)
            {
                searchResult = currentResult;
                return true;
            }

            for (var i = 0; i < currentResult.Control.Children.Count; i++)
            {
                var result = new UiControlSearchResult(currentResult.Control, i);

                if (TryGetControl(result, controlId, out searchResult))
                {
                    return true;
                }
            }

            searchResult = null;
            return false;
        }

        private SizeAnchor GetSizeAnchor(Vector2 vector2)
        {
            if (SelectedControl == null)
            {
                return SizeAnchor.None;
            }

            if (_leftBorderRect.Contains(vector2))
            {
                return SizeAnchor.Left;
            }
            if (_rightBorderRect.Contains(vector2))
            {
                return SizeAnchor.Right;
            }
            if (_topBorderRect.Contains(vector2))
            {
                return SizeAnchor.Top;
            }
            if (_bottomBorderRect.Contains(vector2))
            {
                return SizeAnchor.Bottom;
            }

            return SizeAnchor.None;
        }

        private void UpdateSelectedControl()
        {
            SelectedControl = GetAllControls()
                //.Where(x => GetControlHeriarchy(x.Id).All(c => c.IsVisible))
                .Reverse()
                .FirstOrDefault(x => x.Bounds.Contains(_mousePosition));
        }

        private void DrawAroundBounds(Rectangle bounds, Texture2D texture, int width = 1)
        {
            // top border
            _spriteBatch.Draw(texture,
                new Rectangle(bounds.X - width, bounds.Y - width, bounds.Width + width, width), Color.White);
            // left border
            _spriteBatch.Draw(texture,
                new Rectangle(bounds.X - width, bounds.Y - width, width, bounds.Height + width), Color.White);
            // bottom border
            _spriteBatch.Draw(texture,
                new Rectangle(bounds.X - width, bounds.Bottom, bounds.Width + width, width), Color.White);
            // right border
            _spriteBatch.Draw(texture,
                new Rectangle(bounds.Right, bounds.Y - width, width, bounds.Height + width + width), Color.White);
        }

        private List<Rectangle> CheckForAlignment(Rectangle r1, Rectangle r2, int size)
        {
            var alignmentRects = new List<Rectangle>();
            var adjustmentSize = (int)(size / 2d);

            if (r1.Y == r2.Y || r1.Y == r2.Bottom || r1.Y == r2.Center.Y)
            {
                alignmentRects.Add(new Rectangle(0, r1.Y - adjustmentSize, _drawableBoundsRect.Width, size));
            }
            if (r1.Bottom == r2.Bottom || r1.Bottom == r2.Y || r1.Bottom == r2.Center.Y)
            {
                alignmentRects.Add(new Rectangle(0, r1.Bottom - adjustmentSize, _drawableBoundsRect.Width, size));
            }
            if (r1.Center.Y == r2.Center.Y)
            {
                alignmentRects.Add(new Rectangle(0, r1.Center.Y - adjustmentSize, _drawableBoundsRect.Width, size));
            }

            if (r1.X == r2.X || r1.X == r2.Right || r1.X == r2.Center.X)
            {
                alignmentRects.Add(new Rectangle(r1.X - adjustmentSize, 0, size, _drawableBoundsRect.Height));
            }
            if (r1.Right == r2.Right || r1.Right == r2.X || r1.Right == r2.Center.X)
            {
                alignmentRects.Add(new Rectangle(r1.Right - adjustmentSize, 0, size, _drawableBoundsRect.Height));
            }
            if (r1.Center.X == r2.Center.X)
            {
                alignmentRects.Add(new Rectangle(r1.Center.X - adjustmentSize, 0, size, _drawableBoundsRect.Height));
            }

            return alignmentRects;
        }

        private static bool TryGetControlHeriarchy(GuiControl<IGameToGuiService> control, ulong searchingId, 
            out List<GuiControl<IGameToGuiService>> lowerHeriarchy)
        {
            lowerHeriarchy = [];
            if (control.Id == searchingId)
            {
                return true;
            }

            foreach (var child in control.Children)
            {
                if (!TryGetControlHeriarchy(child, searchingId, out var lowerLevel))
                {
                    continue;
                }

                lowerHeriarchy.Add(control);
                lowerHeriarchy.AddRange(lowerLevel);
                return true;
            }

            return false;
        }

        private static Point ConvertToXnaPoint(System.Windows.Point point)
        {
            return new Point((int)point.X, (int)point.Y);
        }

        private void UpdateCameraDrag(IInputElement reference, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                var newPosition = ConvertToXnaPoint(e.GetPosition(reference));
                if (_isDraggingCamera)
                {
                    var positionDiff = _lastCameraDragMousePosition - newPosition;
                    _cameraHandler.CameraPositionX -= positionDiff.X;
                    _cameraHandler.CameraPositionY -= positionDiff.Y;
                    _lastCameraDragMousePosition = newPosition;
                }
                else
                {
                    _lastCameraDragMousePosition = newPosition;
                    _isDraggingCamera = true;
                }
            }
            else
            {
                _isDraggingCamera = false;
            }
        }

        private void IncreaseControlRenderOrder(GuiControl<IGameToGuiService> customControl)
        {
            if (!TryGetClosestParent(customControl, out var parent))
            {
                return;
            }

            var currentIndex = parent.IndexOf(customControl);
            if (currentIndex == -1)
            {
                Debug.WriteLine("Did not find index of control in parent children list???");
                return;
            }
            if (currentIndex == parent.Children.Count - 1)
            {
                return;
            }

            var newIndex = currentIndex + 1;
            parent.RemoveChild(currentIndex);
            parent.InsertChild(customControl, newIndex);
        }

        private void DecreaseControlRenderOrder(GuiControl<IGameToGuiService> customControl)
        {
            if (!TryGetClosestParent(customControl, out var parent))
            {
                return;
            }


            var currentIndex = parent.IndexOf(customControl);
            if (currentIndex == -1)
            {
                Debug.WriteLine("Did not find index of control in parent children list???");
                return;
            }
            if (currentIndex == 0)
            {
                return;
            }

            var newIndex = currentIndex - 1;
            parent.RemoveChild(currentIndex);
            parent.InsertChild(customControl, newIndex);
        }

        private bool TryGetClosestParent(GuiControl<IGameToGuiService> customControl, out GuiControl<IGameToGuiService> parent)
        {
            if (customControl == null)
            {
                parent = null;
                return false;
            }
            parent = GetControlHeriarchy(customControl.Id).LastOrDefault();
            return parent != null;
        }

        private static void ChangeControlSize(GuiControl<IGameToGuiService> control, SizeAnchor anchor, Vector2 change)
        {
            switch (anchor)
            {
                case SizeAnchor.Top:
                    UpdateControlPosition(control, new Vector2(0, change.Y));

                    control.Size += new Vector2(0, change.Y);
                    return;
                case SizeAnchor.Bottom:
                    control.Size += new Vector2(0, change.Y);
                    return;
                case SizeAnchor.Left:
                    UpdateControlPosition(control, new Vector2(change.X, 0));

                    control.Size += new Vector2(change.X, 0);
                    return;
                case SizeAnchor.Right:
                    control.Size += new Vector2(change.X, 0);
                    return;
            }
        }

        private bool IsKeyClicked(Key key)
        {
            return _keyboard.IsKeyClicked(key);
        }

        private bool AreKeysClicked(params Key[] keys)
        {
            return keys.All(x => _keyboard.IsKeyDown(x))
                && keys.Any(x => _keyboard.IsKeyClicked(x));
        }

        private void PerformSelectedControlKeyboardActions()
        {
            if (IsKeyClicked(Key.OemPlus))
            {
                IncreaseControlRenderOrder(SelectedControl);
            }
            else if (IsKeyClicked(Key.OemMinus))
            {
                DecreaseControlRenderOrder(SelectedControl);
            }

            var isShiftDown = _keyboard.IsKeyDown(Key.LeftShift);
            var shiftValue = Vector2.Zero;
            var anchor = SizeAnchor.None;
            if (IsKeyClicked(Key.Left))
            {
                shiftValue += Vector2.UnitX;
                anchor = SizeAnchor.Left;
            }
            if (IsKeyClicked(Key.Right))
            {
                shiftValue += isShiftDown ? Vector2.UnitX : -Vector2.UnitX;
                anchor = SizeAnchor.Right;
            }
            if (IsKeyClicked(Key.Up))
            {
                shiftValue += Vector2.UnitY;
                anchor = SizeAnchor.Top;
            }
            if (IsKeyClicked(Key.Down))
            {
                shiftValue += isShiftDown ? Vector2.UnitY : -Vector2.UnitY;
                anchor = SizeAnchor.Bottom;
            }
            if (shiftValue == Vector2.Zero)
            {
                return;
            }

            if (isShiftDown)
            {
                ChangeControlSize(SelectedControl, anchor, shiftValue);
                SelectedControlSizeUpdated?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                UpdateControlPosition(SelectedControl, shiftValue);
                SelectedControlPositionUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public void RemoveControl(ulong id)
        {
            if (id == _customControl.Id)
            {
                return;
            }

            if (TryGetControl(id, out var result))
            {
                if (SelectedControl != null && SelectedControl.Id == id)
                {
                    SelectedControl = null;
                }
                result.Remove();
            }
        }

        public void RemoveSelected()
        {
            if (SelectedControl == null)
            {
                return;
            }

            RemoveControl(SelectedControl.Id);
        }

        public void SelectBaseControl()
        {
            SelectedControl = _customControl;
        }

        public void SetSelectedChild(ulong childId)
        {
            if (TryGetControl(childId, out var newSelectedControl))
            {
                SelectedControl = newSelectedControl.Control;
            }
        }

        public void UpdateSelected(GuiControlParameters parameters)
        {
            if (SelectedControl == null)
            {
                return;
            }

            if (!TryGetControl(SelectedControl.Id, out var searchResult))
            {
                return;
            }

            searchResult.Update(parameters, Content, GraphicsDevice, _drawableBoundsRect.Size.ToVector2());
            SelectedControl = searchResult.Control;
        }

        public void ResetWithParameters(GuiControlParameters parameters)
        {
            SelectedControl = null;
            _showIndicator = false;
            _customControl = new GuiControl<IGameToGuiService>(parameters, null);
            _customControl.LoadContent(Content, GraphicsDevice, _drawableBoundsRect.Size.ToVector2());
        }

        public void SelectParent()
        {
            if(SelectedControl == null)
            {
                return;
            }
            if(SelectedControl.Parent == null)
            {
                return;
            }

            SelectedControl = SelectedControl.Parent;
        }

        public List<GuiControl<IGameToGuiService>> GetControlHeriarchy(ulong id)
        {
            if (TryGetControlHeriarchy(_customControl, id, out var heriachy))
            {
                return heriachy;
            }

            return [];
        }

        public IEnumerable<GuiControl<IGameToGuiService>> GetAllControls(ulong? ignoreChildrenOf = null)
        {
            if (!ignoreChildrenOf.HasValue)
            {
                yield return _customControl;
                foreach (var child in GetAllChildrenOf(_customControl))
                {
                    yield return child;
                }
            }
            else if (ignoreChildrenOf != _customControl.Id)
            {
                yield return _customControl;
                foreach (var child in GetAllChildrenOfWithException(_customControl, ignoreChildrenOf.Value))
                {
                    yield return child;
                }
            }
        }

        public static IEnumerable<GuiControl<IGameToGuiService>> GetAllChildrenOf(GuiControl<IGameToGuiService> customControl)
        {
            foreach (var child in customControl.Children)
            {
                yield return child;
                foreach (var childOfChild in GetAllChildrenOf(child))
                {
                    yield return childOfChild;
                }
            }
        }

        public static IEnumerable<GuiControl<IGameToGuiService>> GetAllChildrenOfWithException(GuiControl<IGameToGuiService> customControl, 
            ulong ignoreChildrenOf)
        {
            foreach (var child in customControl.Children)
            {
                if (child.Id == ignoreChildrenOf)
                {
                    continue;
                }

                yield return child;
                foreach (var childOfChild in GetAllChildrenOfWithException(child, ignoreChildrenOf))
                {
                    yield return childOfChild;
                }
            }
        }

        public static void UpdateControlSize(GuiControl<IGameToGuiService> control, Vector2 sizeDiff)
        {
            control.Size -= sizeDiff;
            foreach (var child in control.Children)
            {
                UpdateControlSize(child, sizeDiff);
            }
        }

        public void SetConfiguration(ControlBuilderConfiguration configuration)
        {
            Configuration = configuration;

            var borderParams = new CustomTextureParameters.CustomTextureParametersBuilder()
                .WithSize(new Point(10))
                .WithFillColor(Configuration.SelectedIndicationColor)
                .WithShape(ShapeType.Rectangle);

            _selectedBorderTexture = CustomTextureManager.GetCustomTexture(borderParams.Build(), GraphicsDevice);

            var borderMouseIndicatorParameters = new CustomTextureParameters.CustomTextureParametersBuilder()
                .WithShape(ShapeType.Circle)
                .WithSize(Configuration.SelectedMouseIndicatorWidth)
                .WithFillColor(Configuration.SelectedMouseIndicatorColor);
            _borderMouseIndicator = CustomTextureManager.GetCustomTexture(borderMouseIndicatorParameters.Build(), GraphicsDevice);

            _invisibleControlBorderTexture = CustomTextureManager.GetCustomTexture(
                borderParams.WithFillColor(Configuration.InvisibleControlIndicatorColor).Build(), GraphicsDevice);

            _alignmentTexture = CustomTextureManager.GetCustomTexture(
                borderParams.WithFillColor(Configuration.AlignmentIndicationColor).Build(), GraphicsDevice);

            _boundsIndicator = CustomTextureManager.GetCustomTexture(
                borderParams.WithFillColor(Configuration.BoundsIndicatorColor).Build(), GraphicsDevice);
        }

        public override void OnMouseMove(IInputElement reference, MouseEventArgs e)
        {
            _mousePosition = _cameraHandler.ScreenToWorld(ConvertToXnaPoint(e.GetPosition(reference)).ToVector2());
            UpdateIndicator();
            UpdateCameraDrag(reference, e);
            if (_currentDragMode == DragMode.None)
            {
                return;
            }
            if (SelectedControl == null)
            {
                _currentDragMode = DragMode.None;
                return;
            }

            var dragDiff = _mouseDragPositionStart - _mousePosition;
            var oldPosition = SelectedControl.Position;
            if (_currentDragMode == DragMode.Position)
            {
                var newPosition = _controlDragPositionStart - dragDiff;
                if (Configuration.UseSnapAlignment && !_keyboard.IsKeyDown(Key.LeftAlt))
                {
                    var newBounds = new Rectangle(newPosition.ToPoint(), SelectedControl.Size.ToPoint());

                    var bestAlignment = GetAllControls(SelectedControl.Id)
                        .Select(x => new AlignmentModel(x.Bounds, newBounds, Configuration.SnapAlignmentRange))
                        .Where(x => x.IsWithinSnapRange)
                        .OrderBy(x => x.DistanceToAlignment)
                        .FirstOrDefault();

                    if (bestAlignment != null)
                    {
                        newPosition = bestAlignment.NewPosition;
                    }
                }

                var positionDiff = oldPosition - newPosition;
                UpdateControlPosition(SelectedControl, positionDiff);
                SelectedControlPositionUpdated?.Invoke(this, EventArgs.Empty);
                return;
            }

            var oldSize = SelectedControl.Size;
            switch (_currentSizeAnchor)
            {
                case SizeAnchor.Top:
                    var newTopPosition = _controlDragPositionStart - new Vector2(0, dragDiff.Y);
                    var positionDiffTop = oldPosition - newTopPosition;
                    UpdateControlPosition(SelectedControl, positionDiffTop);
                    SelectedControlPositionUpdated?.Invoke(this, EventArgs.Empty);

                    var newSizeTop = _controlDragSizeStart + new Vector2(0, dragDiff.Y);
                    SelectedControl.Size = newSizeTop;
                    SelectedControlSizeUpdated?.Invoke(this, EventArgs.Empty);
                    return;
                case SizeAnchor.Bottom:
                    var newSizeBottom = _controlDragSizeStart - new Vector2(0, dragDiff.Y);
                    SelectedControl.Size = newSizeBottom;
                    SelectedControlSizeUpdated?.Invoke(this, EventArgs.Empty);
                    return;
                case SizeAnchor.Left:
                    var newLeftPosition = _controlDragPositionStart - new Vector2(dragDiff.X, 0);
                    var positionDiffLeft = oldPosition - newLeftPosition;
                    UpdateControlPosition(SelectedControl, positionDiffLeft);
                    SelectedControlPositionUpdated?.Invoke(this, EventArgs.Empty);

                    var newSizeLeft = _controlDragSizeStart + new Vector2(dragDiff.X, 0);
                    SelectedControl.Size = newSizeLeft;
                    SelectedControlSizeUpdated?.Invoke(this, EventArgs.Empty);
                    return;
                case SizeAnchor.Right:
                    var newSizeRight = _controlDragSizeStart - new Vector2(dragDiff.X, 0);
                    SelectedControl.Size = newSizeRight;
                    SelectedControlSizeUpdated?.Invoke(this, EventArgs.Empty);
                    return;
            }
        }

        public override void OnMouseWheel(IInputElement reference, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                _cameraHandler.Scale += .1f;
            }
            else
            {
                _cameraHandler.Scale -= .1f;
            }
        }

        public override void OnMouseDown(IInputElement reference, MouseButtonEventArgs e)
        {
            _mousePosition = _cameraHandler.ScreenToWorld(ConvertToXnaPoint(e.GetPosition(reference)).ToVector2());
            if(e.ChangedButton != MouseButton.Left || SelectedControl == null)
            {
                return;
            }

            if (SelectedControl.Bounds.Contains(_mousePosition))
            {
                var currentTime = DateTime.Now;
                if ((currentTime - _lastSelectedControlClick).TotalMilliseconds < 500)
                {
                    //SelectedControlDoubleClicked?.Invoke(this, EventArgs.Empty);
                    return;
                }
                _lastSelectedControlClick = currentTime;
            }

            var sizeAnchor = GetSizeAnchor(_mousePosition);
            if (sizeAnchor != SizeAnchor.None)
            {
                _currentSizeAnchor = sizeAnchor;
                _showIndicator = true;
                _currentDragMode = DragMode.Size;
                _mouseDragPositionStart = _mousePosition;
                _controlDragPositionStart = SelectedControl.Position;
                _controlDragSizeStart = SelectedControl.Size;
            }
            else if (SelectedControl.Bounds.Contains(_mousePosition))
            {
                _showIndicator = false;
                _currentDragMode = DragMode.Position;
                _mouseDragPositionStart = _mousePosition;
                _controlDragPositionStart = SelectedControl.Position;
            }
            else
            {
                _showIndicator = false;
                _currentDragMode = DragMode.None;
            }
        }

        public override void OnMouseUp(IInputElement reference, MouseButtonEventArgs e)
        {
            _mousePosition = _cameraHandler.ScreenToWorld(ConvertToXnaPoint(e.GetPosition(reference)).ToVector2());
            if(e.ChangedButton == MouseButton.Right)
            {
                SelectedControl = null;
                _showIndicator = false;
            }

            if(e.ChangedButton == MouseButton.Left)
            {
                if (SelectedControl == null)
                {
                    UpdateSelectedControl();
                }
                _currentDragMode = DragMode.None;
            }
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _cameraHandler = new CameraHandler(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            SetDrawableArea();

            SetConfiguration(UiBuilderConfigurationHelper.GetConfiguration("default"));

            _font = Content.Load<SpriteFont>("Fonts/DefaultFont");

            IsInitialized = true;
            _customControl = new GuiControl<IGameToGuiService>(new GuiControlParameters(_drawableBoundsRect.Size.ToVector2()) 
            {
                IsVisible = true,
                IsEnabled = true
            }, null);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Configuration.BackgroundColor);
            _spriteBatch.Begin(transformMatrix: _cameraHandler.ViewMatrix);
            DrawAroundBounds(_drawableBoundsRect, _boundsIndicator, 50);

            if (_customControl == null)
            {
                _spriteBatch.End();
                return;
            }

            if (_customControl.IsVisible)
            {
                _customControl.Draw(_spriteBatch, _cameraHandler);
            }
            else
            {
                DrawAroundBounds(_customControl.Bounds, _invisibleControlBorderTexture, Configuration.InvisibleControlIndicatorWidth);
            }

            foreach (var child in GetAllChildrenOf(_customControl).Where(x => !x.IsVisible)) //_customControl.Children.Where(x => !x.IsVisible))
            {
                DrawAroundBounds(child.Bounds, _invisibleControlBorderTexture, Configuration.InvisibleControlIndicatorWidth);
            }

            if (SelectedControl != null)
            {
                DrawAroundBounds(SelectedControl.Bounds, _selectedBorderTexture, Configuration.SelectedIndicationWidth);

                if (_showIndicator)
                {
                    _spriteBatch.Draw(_borderMouseIndicator,
                        _borderMouseIndicator.Bounds.CenterOnVector(_mousePosition), Color.White);
                }

                if (Configuration.ShowSelectedPositionAndSize)
                {
                    var selectedControlsString = $"Position: {SelectedControl.Position.X}x{SelectedControl.Position.Y}\n" +
                        $"Size: {SelectedControl.Size.X}x{SelectedControl.Size.Y}";

                    _spriteBatch.DrawString(_font, selectedControlsString,
                        _cameraHandler.ScreenToWorld(Vector2.Zero), Color.White, 0, Vector2.Zero,
                        1 / _cameraHandler.Scale, SpriteEffects.None, 0);
                }
            }

            foreach (var alignmentIndication in _alignmentIndicators)
            {
                _spriteBatch.Draw(_alignmentTexture, alignmentIndication, Color.White);
            }

            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _keyboard.Update();

            if (_cameraHandler.ScreenWidth != GraphicsDevice.Viewport.Width 
                || _cameraHandler.ScreenHeight != GraphicsDevice.Viewport.Height)
            {
                _cameraHandler = new CameraHandler(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)
                {
                    CameraPosition = _cameraHandler.CameraPosition,
                    Scale = _cameraHandler.Scale
                };
            }

            if (GetAllControls().Any(x => x.Background != null && x.Background.IsDisposed))
            {
                ResetWithParameters(Parameters);
            }

            _cameraHandler.Update();

            if (_customControl == null)
            {
                return;
            }

            _customControl.Update(gameTime, _cameraHandler);
            _alignmentIndicators.Clear();

            if (SelectedControl == null)
            {
                if (_copyParameters != null
                   && AreKeysClicked(Key.LeftCtrl, Key.V))
                {
                    Parameters.ChildControls.Add(_copyParameters.Copy());
                    ResetWithParameters(Parameters);
                }
                return;
            }
            if (AreKeysClicked(Key.LeftCtrl, Key.C))
            {
                _copyParameters = SelectedControl.Parameters.Copy();
            }
            if (_copyParameters != null
                   && AreKeysClicked(Key.LeftCtrl, Key.V))
            {
                Parameters.ChildControls.Add(_copyParameters.Copy());
                ResetWithParameters(Parameters);
                return;
            }

            if (IsKeyClicked(Key.Delete))
            {
                RemoveSelected();
                ControlChildrenUpdated?.Invoke(this, EventArgs.Empty);
                return;
            }

            PerformSelectedControlKeyboardActions();

            var bounds = SelectedControl.Bounds;
            _leftBorderRect = new Rectangle(bounds.Location, new Point(3, bounds.Height));
            _rightBorderRect = new Rectangle(new Point(bounds.Right - 3, bounds.Y), new Point(3, bounds.Height));
            _topBorderRect = new Rectangle(bounds.Location, new Point(bounds.Width, 3));
            _bottomBorderRect = new Rectangle(new Point(bounds.X, bounds.Bottom - 3), new Point(bounds.Width, 3));

            if (_currentDragMode == DragMode.None)
            {
                return;
            }

            _alignmentIndicators.AddRange(GetAllControls(SelectedControl.Id)
                .SelectMany(x => CheckForAlignment(bounds, x.Bounds, Configuration.AlignmentIndicationWidth)));

            _alignmentIndicators.AddRange(CheckForAlignment(bounds, _drawableBoundsRect, Configuration.AlignmentIndicationWidth));
        }

        public event EventHandler ControlChildrenUpdated;
        public event EventHandler SelectedControlChanged;
        public event EventHandler SelectedControlSizeUpdated;
        public event EventHandler SelectedControlPositionUpdated;
    }
}