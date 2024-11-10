using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oscetch.MonoGame.GuiComponent.Models
{
    public class DragAndDropItem(object item, Texture2D texture, Vector2 size)
    {
        public object Item { get; } = item;
        public Texture2D Texture { get; } = texture;
        public Vector2 Size { get; } = size;
    }
}
