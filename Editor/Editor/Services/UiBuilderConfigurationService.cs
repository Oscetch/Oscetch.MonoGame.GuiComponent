using Editor.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Editor.Services
{
    public static class UiBuilderConfigurationHelper
    {
        private const string _uiBuilderConfigurationFileName = "Config/uiBuilderConfigration.json";
        private static readonly Dictionary<string, ControlBuilderConfiguration> _configurations = File.Exists(_uiBuilderConfigurationFileName)
            ? JsonConvert.DeserializeObject<Dictionary<string, ControlBuilderConfiguration>>(File.ReadAllText(_uiBuilderConfigurationFileName))
            : new Dictionary<string, ControlBuilderConfiguration>();

        public static void AddOrUpdateConfiguration(string name, ControlBuilderConfiguration configuration)
        {
            if (string.IsNullOrEmpty(name) || configuration == null)
            {
                return;
            }

            var directoryPath = Path.GetDirectoryName(_uiBuilderConfigurationFileName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            _configurations[name] = configuration;
            File.Delete(_uiBuilderConfigurationFileName);
            File.WriteAllText(_uiBuilderConfigurationFileName, JsonConvert.SerializeObject(_configurations));
        }

        public static ControlBuilderConfiguration GetConfiguration(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            if (_configurations.TryGetValue(name, out var configuration))
            {
                return configuration;
            }

            var newConfiguration = new ControlBuilderConfiguration();
            AddOrUpdateConfiguration(name, newConfiguration);

            return newConfiguration;
        }
    }
}
