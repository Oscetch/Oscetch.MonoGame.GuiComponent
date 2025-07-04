using Newtonsoft.Json;
using System.IO;

namespace Editor
{
    public class ProjectSettings
    {
        private static ProjectSettings _settings;

        public static ProjectSettings GetSettings(string path = "")
        {
            if (!string.IsNullOrEmpty(path) && _settings?.SettingsPath != path)
            {
                _settings = null;
            }

            if(_settings != null)
            {
                return _settings;
            }
            try
            {
                _settings = JsonConvert.DeserializeObject<ProjectSettings>(File.ReadAllText(path));
            }
            catch
            {
                _settings = new ProjectSettings
                {
                    SettingsPath = string.IsNullOrEmpty(path) ? "settings.oscetchjson" : path
                };
            }
            return _settings;
        }

        public static void Save()
        {
            if(_settings != null)
            {
                File.WriteAllText(_settings.SettingsPath, JsonConvert.SerializeObject(_settings));
            }
        }

        private ProjectSettings() { }

        public string SettingsPath { get; set; }
        public string AssemblyName { get; set; } = "Default";
        public string OutputPath { get; set; } = "Osetch.Canvas.dll";
        public string ScriptsDir { get; set; } = "scripts";
        public string TestExePath { get; set; } = null;
        public string TestJsonPath { get; set; } = null;
        public string ContentBinPath { get; set; } = null;
        public string FontPath { get; set; } = null;
        public string MgcbPath { get; set; } = null;
        public string ContentPath { get; set; } = null;
        public string GameDllPath { get; set; } = null;
    }
}
