using Oscetch.MonoGame.Textures;

namespace Editor.Extensions
{
    public static class CustomTextureParametersExtensions
    {
        public static CustomTextureParameters.CustomTextureParametersBuilder ToBuilder(this CustomTextureParameters textureParams)
        {
            var builder = new CustomTextureParameters.CustomTextureParametersBuilder()
                    .WithShape(textureParams.ShapeType)
                    .WithSize(textureParams.Size)
                    .WithFillColor(textureParams.FillColor)
                    .WithCornerRadius(textureParams.CornerRadius);
            if (textureParams.IsBordered)
            {
                builder.WithBorderThickness(textureParams.BorderThickness).WithBorderColor(textureParams.BorderColor);
            }
            return builder;
        }
    }
}
