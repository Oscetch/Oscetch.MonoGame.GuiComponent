using Microsoft.Xna.Framework;
using Oscetch.ScriptComponent;
using System.Collections.Generic;
using System.Linq;

namespace Oscetch.MonoGame.GuiComponent.Models
{
    public class GuiControlParameters(Vector2 originalResolution)
    {
        public string Name { get; set; }
        public Vector2 OriginalResolution { get; private set; } = originalResolution;
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 TextPosition { get; set; }
        public float TextScale { get; set; } = 1.0f;
        public float TextRotation { get; set; } = 0.0f;

        public bool IsModal { get; set; } = false;

        public bool CenterText { get; set; }
        public bool IsVisible { get; set; }
        public bool IsEnabled { get; set; }
        public bool Clip { get; set; } = false;
        public Background Background { get; set; }
        public Color TextColor { get; set; } = Color.White;
        public string Text { get; set; }
        public string SpriteFont { get; set; } = "Fonts/DefaultFont";

        public bool HasBorder { get; set; }
        public Color BorderColor { get; set; } = Color.White;

        public List<ScriptReference> ControlScripts { get; set; } = [];
        public List<ScriptReference> BuiltInScripts { get; set; } = [];
        public List<GuiControlParameters> ChildControls { get; set; } = [];

        internal void UpdateResolution(Vector2 newResolution)
        {
            var scale = newResolution / OriginalResolution;
            Size *= scale;
            Position *= scale;
            TextPosition *= scale;
            TextScale *= scale.Y;

            OriginalResolution = newResolution;
        }

        public GuiControlParameters Copy()
        {
            return new GuiControlParameters(OriginalResolution)
            {
                Position = Position,
                TextPosition = TextPosition,
                CenterText = CenterText,
                Size = Size,
                Background = Background?.Copy(),
                TextColor = TextColor,
                Text = Text,
                SpriteFont = SpriteFont,
                ChildControls = [.. ChildControls.Select(x => x.Copy())],
                ControlScripts = [.. ControlScripts],
                BuiltInScripts = [.. BuiltInScripts],
                IsEnabled = IsEnabled,
                IsVisible = IsVisible,
                Name = Name,
                TextScale = TextScale,
                TextRotation = TextRotation,
                BorderColor = BorderColor,
                HasBorder = HasBorder,
                IsModal = IsModal,
                Clip = Clip,
            };
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
