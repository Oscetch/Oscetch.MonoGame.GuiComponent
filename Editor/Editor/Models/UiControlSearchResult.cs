using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.MonoGame.GuiComponent;
using Oscetch.MonoGame.GuiComponent.Interfaces;
using Oscetch.MonoGame.GuiComponent.Models;

namespace Editor.Models
{
    public class UiControlSearchResult(GuiControl<IGameToGuiService> parent, int index)
    {
        private readonly GuiControl<IGameToGuiService> _parent = parent;
        private readonly int _index = index;

        public GuiControl<IGameToGuiService> Control => _parent.Children[_index];

        public void Update(GuiControlParameters parameters, ContentManager contentManager, 
            Vector2 resolution)
        {
            GuiControl<IGameToGuiService> newChild = new(parameters, null);
            newChild.LoadContent(contentManager, resolution);
            _parent.ReplaceChild(newChild, _index);
        }

        public void Remove()
        {
            _parent.RemoveChild(_index);
        }
    }
}
