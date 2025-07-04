using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Editor.MgcbStuff
{
    internal static class Mgcb
    {
        public static void Rebuild(string customContentPath = null, string customMgcbPath = null)
        {
            var contentPath = Path.GetFullPath(customContentPath ?? ProjectSettings.GetSettings().ContentBinPath);
            var mgcbPath = Path.GetFullPath(customMgcbPath) ?? ProjectSettings.GetSettings().MgcbPath;

            RunCommand(contentPath, mgcbPath, "/@:\"Content.mgcb\" /rebuild");
        }

        public static void RunCommand(string contentPath, string mgcbPath, string args)
        {
            var psi = new ProcessStartInfo
            {
                FileName = mgcbPath,
                WorkingDirectory = contentPath,
                Arguments = args,
                RedirectStandardOutput = true,  
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };
            using var proc = Process.Start(psi);
            var stdOut = proc.StandardOutput.ReadToEnd();
            var stdError = proc.StandardError.ReadToEnd();

            Debug.WriteLine("=== MGCB OUTPUT ===");
            Debug.WriteLine(stdOut);

            if (!string.IsNullOrEmpty(stdError))
            {
                Debug.WriteLine("=== MGCB ERROR ===");
                Debug.WriteLine(stdError);
            }

            proc.WaitForExit();
        }

        public static void AddTexture(string texturePath, string customContentPath = null, string customMgcbPath = null) =>
            AddFileToContent(texturePath, TEXTURE2D, customContentPath, customMgcbPath);

        public static void AddSpriteFont(string spriteFontPath, string customContentPath = null, string customMgcbPath = null) =>
            AddFileToContent(spriteFontPath, FONT, customContentPath, customMgcbPath);

        private static void AddFileToContent(string newFilePath, string template, string customContentPath = null, string customMgcbPath = null)
        {
            var contentPath = Path.GetFullPath(customContentPath ?? ProjectSettings.GetSettings().ContentBinPath);
            var fullTexturePath = Path.GetFullPath(newFilePath);
            var fileName = Path.GetFileName(fullTexturePath);
            var path = Path.Join(contentPath, "Content.mgcb");

            string finalTexturePath;
            if (!fullTexturePath.StartsWith(contentPath))
            {
                finalTexturePath = Path.Join(contentPath, fileName);
                File.Copy(fullTexturePath, finalTexturePath, true);
            }
            else
            {
                finalTexturePath = fullTexturePath;
            }

            var contentName = Path.GetRelativePath(contentPath, finalTexturePath);

            File.AppendAllLines(path, string.Format(template, contentName).Split(separator: ["\r\n", "\n", "\r"], System.StringSplitOptions.None));

            Rebuild(contentPath, customMgcbPath);
        }

        private const string TEXTURE2D = @"
#begin {0}
/importer:TextureImporter
/processor:TextureProcessor
/processorParam:ColorKeyColor=255,0,255,255
/processorParam:ColorKeyEnabled=True
/processorParam:GenerateMipmaps=False
/processorParam:PremultiplyAlpha=True
/processorParam:ResizeToPowerOfTwo=False
/processorParam:MakeSquare=False
/processorParam:TextureFormat=Color
/build:{0}
";

        private const string FONT = @"
#begin {0}
/importer:FontDescriptionImporter
/processor:FontDescriptionProcessor
/processorParam:PremultiplyAlpha=True
/processorParam:TextureFormat=Compressed
/build:{0}
";
    }
}
