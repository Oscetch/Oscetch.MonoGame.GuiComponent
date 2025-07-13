using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Oscetch.MonoGame.Textures;
using System.IO;

namespace Oscetch.MonoGame.GuiComponent.Models
{
    public class Background
    {
        public string Path { get; }
        public bool IsBuiltTexture { get; }
        public CustomTextureParameters CustomTextureParameters { get; }

        [JsonConstructor]
        public Background(string path, CustomTextureParameters customTextureParameters, bool isBuiltTexture)
        {
            Path = path;
            CustomTextureParameters = customTextureParameters;
            IsBuiltTexture = isBuiltTexture;
        }

        public Background(string path)
        {
            Path = path;
            IsBuiltTexture = false;
        }

        public Background(CustomTextureParameters customTextureParameters)
        {
            IsBuiltTexture = true;
            CustomTextureParameters = customTextureParameters;
        }

        public Background Copy() => new(Path, CopyParameters(), IsBuiltTexture);

        public Texture2D Load(ContentManager contentManager, GraphicsDevice graphicsDevice)
        { 
            if (IsBuiltTexture)
            {
                return CustomTextureManager.GetCustomTexture(CustomTextureParameters, graphicsDevice);
            }

            if (string.IsNullOrEmpty(Path))
            {
                return null;
            }

            if (!System.IO.Path.HasExtension(Path))
            {
                return contentManager.Load<Texture2D>(Path);
            }
            if (!File.Exists(Path))
            {
                return null;
            }

            using var fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
            return Texture2D.FromStream(graphicsDevice, fileStream);
        }

        private CustomTextureParameters CopyParameters()
        {
            if (CustomTextureParameters == null) return null;

            var builder = new CustomTextureParameters.CustomTextureParametersBuilder()
                    .WithShape(CustomTextureParameters.ShapeType)
                    .WithSize(CustomTextureParameters.Size)
                    .WithFillColor(CustomTextureParameters.FillColor)
                    .WithCornerRadius(CustomTextureParameters.CornerRadius);
            if (CustomTextureParameters.IsBordered)
            {
                builder.WithBorderThickness(CustomTextureParameters.BorderThickness).WithBorderColor(CustomTextureParameters.BorderColor);
            }
            return builder.Build();
        }

        public static implicit operator Background(string path) => new(path);
        public static implicit operator Background(CustomTextureParameters parameters) => new(parameters);
    }
}
