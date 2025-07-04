namespace Editor.Extensions
{
    public static class ColorExtensions
    {
        public static System.Windows.Media.Color ToWindows(this Microsoft.Xna.Framework.Color color) => new()
        {
            R = color.R,
            G = color.G,
            B = color.B,
            A = color.A
        };

        public static Microsoft.Xna.Framework.Color ToXna(this System.Windows.Media.Color color) => new (color.R, color.G, color.B, color.A);
    }
}
