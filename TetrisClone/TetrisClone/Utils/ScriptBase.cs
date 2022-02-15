using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.MonoGame.Camera;
using Oscetch.MonoGame.GuiComponent;
using Oscetch.MonoGame.GuiComponent.Interfaces;
using Oscetch.MonoGame.GuiComponent.Models;

namespace TetrisClone.Utils
{
    public abstract class ScriptBase : IGuiScript<GameToGuiService>
    {
        public virtual void Initialize(GuiControl<GameToGuiService> control,
            ContentManager contentManager,
            GraphicsDevice graphicsDevice,
            GameToGuiService gameToGuiService)
        {
        }

        public virtual void LeftClick()
        {
        }

        public virtual void MouseEnter()
        {
        }

        public virtual void MouseExit()
        {
        }

        public virtual DragAndDropItem OnDrag()
        {
            return null;
        }

        public virtual void RightClick()
        {
        }

        public virtual bool TryDrop(DragAndDropItem dragAndDropItem)
        {
            return false;
        }

        public virtual void Update(GameTime gameTime, CameraHandler cameraHandler)
        {
        }

        public virtual void VisibleChanged(bool isVisible)
        {
        }
    }
}
