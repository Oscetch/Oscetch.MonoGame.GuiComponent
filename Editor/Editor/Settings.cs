using Editor.Models;
using Microsoft.CodeAnalysis.Classification;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Editor
{
    public class Settings
    {
        private const string SETTINGS_FILE_PATH = "settings.json";

        private static Settings _settings;

        public static Settings GetSettings()
        {
            if(_settings != null)
            {
                return _settings;
            }

            try
            {
                _settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SETTINGS_FILE_PATH));
            }
            catch
            {
                _settings = new Settings();
            }
            return _settings;
        }

        public static void Save()
        {
            if(_settings != null)
            {
                File.WriteAllText(SETTINGS_FILE_PATH, JsonConvert.SerializeObject(_settings));
            }
        }

        private Settings() { }

        public string AssemblyName { get; set; } = "Default";
        public string BuildDirectory { get; set; } = Environment.CurrentDirectory;
        public string OutputPath { get; set; } = "Default.dll";
        public string ScriptsDir { get; set; } = "scripts";
    }
}
