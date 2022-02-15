using Editor.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.MonoGame.GuiComponent;
using Oscetch.MonoGame.GuiComponent.Models;
using TetrisClone;

namespace Editor.Models
{
    public class UiControlSearchResult
    {
        private readonly GuiControl<GameToGuiService> _parent;
        private readonly int _index;

        public GuiControl<GameToGuiService> Control => _parent.Children[_index];

        public UiControlSearchResult(GuiControl<GameToGuiService> parent, int index)
        {
            _parent = parent;
            _index = index;
        }

        public void Update(GuiControlParameters parameters, ContentManager contentManager, GraphicsDevice graphicsDevice, 
            Vector2 resolution)
        {
            GuiControl<GameToGuiService> newChild = new(parameters, null);
            newChild.LoadContent(contentManager, graphicsDevice, resolution);
            _parent.ReplaceChild(newChild, _index);
        }

        public void Remove()
        {
            _parent.RemoveChild(_index);
        }
    }
}
