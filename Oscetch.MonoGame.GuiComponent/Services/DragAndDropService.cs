using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.GuiComponent.Enums;
using Oscetch.MonoGame.GuiComponent.Interfaces;
using Oscetch.MonoGame.GuiComponent.Models;
using Oscetch.MonoGame.Input.Managers;
using System.Collections.Generic;
using System.Linq;

namespace Oscetch.MonoGame.GuiComponent.Services
{
    public class DragAndDropService<T>(GuiCanvas<T> canvas) where T : IGameToGuiService
    {
        private readonly GuiCanvas<T> _canvas = canvas;

        private static List<DragAndDropItem> _currentItems;
        private Vector2 _lastPosition = Vector2.Zero;
        private bool _waitingOnDrag = false;

        public static bool IsDraging => _currentItems != null;

        public void Update()
        {
            var isCurrentItemNull = _currentItems == null;
            if (isCurrentItemNull && MouseManager.IsLeftButtonPressed)
            {
                if (!_waitingOnDrag)
                {
                    _lastPosition = MouseManager.RawPosition.ToVector2();
                    _waitingOnDrag = true;
                    return;
                }

                if (MouseManager.RawPosition.ToVector2() == _lastPosition)
                {
                    return;
                }

                Drag();
            }
            else if (!isCurrentItemNull && !MouseManager.IsLeftButtonPressed)
            {
                Drop();
            }

            _waitingOnDrag = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentItems == null)
            {
                return;
            }

            foreach (var dropItem in _currentItems)
            {
                spriteBatch.Draw(dropItem.Texture,
                    new Rectangle(MouseManager.RawPosition, dropItem.Size.ToPoint()), Color.White);
            }
        }

        private void Drag()
        {
            for (var i = _canvas.Controls.Count - 1; i >= 0; i--)
            {
                var control = _canvas.Controls[i];
                if (!control.IsEnabled)
                {
                    continue;
                }
                if (!TryDrag(control, out var dragAndDropItems))
                {
                    continue;
                }

                _currentItems = dragAndDropItems;
                break;
            }
        }

        private static bool TryDrag(GuiControl<T> control, out List<DragAndDropItem> items)
        {
            items = null;

            if (control.MouseOverGameObjectState == MouseOverControlState.Over)
            {
                var dragItems = control.LoadedScripts.Select(x => x.OnDrag()).Where(x => x != null && x.Item != null).ToList();
                if (dragItems.Count != 0)
                {
                    items = dragItems;
                    return true;
                }
            }

            if (control.Children.Count == 0)
            {
                return false;
            }

            for (var i = control.Children.Count - 1; i >= 0; i--)
            {
                var child = control.Children[i];
                if (TryDrag(child, out items))
                {
                    return true;
                }
            }

            return false;
        }

        private void Drop()
        {
            for (var i = _canvas.Controls.Count - 1; i >= 0; i--)
            {
                var control = _canvas.Controls[i];
                if (!control.IsEnabled)
                {
                    continue;
                }

                if (!TryDrop(control))
                {
                    continue;
                }

                break;
            }

            _currentItems = null;
        }

        private static bool TryDrop(GuiControl<T> control)
        {
            if (control.MouseOverGameObjectState == MouseOverControlState.Over)
            {
                if (control.LoadedScripts.Any(x => _currentItems.Any(y => x.TryDrop(y))))
                {
                    return true;
                }
            }

            if (control.Children.Count == 0)
            {
                return false;
            }

            for (var i = control.Children.Count - 1; i >= 0; i--)
            {
                var child = control.Children[i];
                if (TryDrop(child))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
