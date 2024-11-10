using Newtonsoft.Json;
using Oscetch.ScriptComponent;
using System.IO;

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
        public string OutputPath { get; set; } = "Osetch.Canvas.dll";
        public string ScriptsDir { get; set; } = "scripts";
        public string TestExePath { get; set; } = null;
        public string TestJsonPath { get; set; } = null;
        public string ContentPath { get; set; } = null;
        public ScriptReference BaseScriptReference { get; set; } = null;
    }
}
