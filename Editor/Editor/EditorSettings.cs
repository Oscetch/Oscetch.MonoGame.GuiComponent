using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Editor
{
    public class EditorSettings
    {
        private const string PATH = "editor_settings.json";

        private static EditorSettings _instance;

        public static EditorSettings Load()
        {
            if (_instance != null) return _instance;

            try
            {
                _instance = JsonConvert.DeserializeObject<EditorSettings>(File.ReadAllText(PATH));
            }
            catch
            {
                _instance = new EditorSettings();
            }
            return _instance;
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText(PATH, json);
        }

        private EditorSettings() { }

        public void LoadProject(string path)
        {
            if (!KnownProjectPaths.Contains(path))
            {
                KnownProjectPaths.Add(path);
            }
            LastProjectPath = path;
            Save();
            ProjectChanged?.Invoke(this, EventArgs.Empty);
        }

        public void InvokeRecreateEditor()
        {
            RecreateEditor?.Invoke(this, EventArgs.Empty);
        }

        public List<string> KnownProjectPaths { get; set; } = [];
        public string LastProjectPath { get; set; } = string.Empty;

        public event EventHandler ProjectChanged;
        public event EventHandler RecreateEditor;
    }
}
