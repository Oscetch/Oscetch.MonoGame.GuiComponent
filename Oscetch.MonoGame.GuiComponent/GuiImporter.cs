using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using Oscetch.MonoGame.GuiComponent.Models;
using System.Collections.Generic;
using System.IO;

namespace Oscetch.MonoGame.GuiComponent
{
    public class GuiImporter : ContentImporter<List<GuiControlParameters>>
    {
        [ContentImporter(".gui", DisplayName = "Oscetch GUI importer", DefaultProcessor = "PassThroughProcessor", CacheImportedData = false)]
        public override List<GuiControlParameters> Import(string filename, ContentImporterContext context)
        {
            using var resourceStream = TitleContainer.OpenStream(filename);
            var json = new StreamReader(resourceStream).ReadToEnd();
            return JsonConvert.DeserializeObject<List<GuiControlParameters>>(json);
        }
    }
}
