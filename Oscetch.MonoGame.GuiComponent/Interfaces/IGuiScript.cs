using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.MonoGame.Camera;
using Oscetch.MonoGame.GuiComponent.Models;
using Oscetch.ScriptComponent.Interfaces;

namespace Oscetch.MonoGame.GuiComponent.Interfaces
{
    public interface IGuiScript<T> : IScript where T : IGameToGuiService
    {
        void VisibleChanged(bool isVisible);
        void LeftClick();
        void RightClick();
        void MouseEnter();
        void MouseExit();
        void Update(GameTime gameTime, CameraHandler cameraHandler);
        void Initialize(GuiControl<T> control, ContentManager contentManager, GraphicsDevice graphicsDevice);
        DragAndDropItem OnDrag();
        bool TryDrop(DragAndDropItem dragAndDropItem);
    }
}
