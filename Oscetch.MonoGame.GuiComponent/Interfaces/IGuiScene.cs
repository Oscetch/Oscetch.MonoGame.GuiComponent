using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.ScriptComponent.Interfaces;

namespace Oscetch.MonoGame.GuiComponent.Interfaces
{
    public interface IGuiScene : IScript
    {
        /// <summary>
        /// Is set automatically by the editor
        /// </summary>
        public GuiCanvas<IGameToGuiService> Canvas { get; set; }

        /// <summary>
        /// Called after `Initialize`
        /// </summary>
        /// <returns></returns>
        public IGameToGuiService CreateGameToGuiService();
        public void Initialize(ContentManager content, GraphicsDevice graphicsDevice, bool isEditor = false);

        public void Update(GameTime gameTime);
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
