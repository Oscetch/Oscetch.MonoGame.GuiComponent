using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oscetch.MonoGame.GuiComponent.Models
{
    public class DragAndDropItem
    {
        public object Item { get; }
        public Texture2D Texture { get; }
        public Vector2 Size { get; }

        public DragAndDropItem(object item, Texture2D texture, Vector2 size)
        {
            Item = item;
            Texture = texture;
            Size = size;
        }
    }
}
