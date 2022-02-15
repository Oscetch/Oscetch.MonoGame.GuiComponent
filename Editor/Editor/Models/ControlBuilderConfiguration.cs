using Microsoft.Xna.Framework;

namespace Editor.Models
{
    public class ControlBuilderConfiguration
    {
        public int AlignmentIndicationWidth { get; set; } = 1;
        public Color AlignmentIndicationColor { get; set; } = Color.Purple;
        public int SelectedIndicationWidth { get; set; } = 1;
        public Color SelectedIndicationColor { get; set; } = Color.Yellow;
        public int SelectedMouseIndicatorWidth { get; set; } = 10;
        public Color SelectedMouseIndicatorColor { get; set; } = Color.Yellow;
        public int InvisibleControlIndicatorWidth { get; set; } = 1;
        public Color InvisibleControlIndicatorColor { get; set; } = Color.Blue;
        public Color BackgroundColor { get; set; } = Color.Black;
        public Color BoundsIndicatorColor { get; set; } = Color.White;
        public bool ShowSelectedPositionAndSize { get; set; } = true;
        public bool UseSnapAlignment { get; set; } = true;
        public int SnapAlignmentRange { get; set; } = 10;
    }
}
