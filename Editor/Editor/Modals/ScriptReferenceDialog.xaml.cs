using Editor.Extensions;
using Editor.Models;
using Oscetch.MonoGame.GuiComponent.Interfaces;
using Oscetch.ScriptComponent;
using Oscetch.ScriptComponent.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Editor.Modals
{
    /// <summary>
    /// Interaction logic for ListDialog.xaml
    /// </summary>
    public partial class ScriptReferenceDialog : Window
    {
        private readonly ProjectSettings _settings;

        public ObservableCollection<ScriptReferenceCheckedModel> ScriptReferenceCheckedModels { get; set; }

        public ScriptReferenceDialog(List<ScriptReference> currentReferences)
        {
            _settings = ProjectSettings.GetSettings();
            InitializeComponent();

            Title = "Script References";

            ScriptReferenceCheckedModels = [];
            foreach(var currentReference in currentReferences)
            {
                ScriptReferenceCheckedModels.Add(new ScriptReferenceCheckedModel(currentReference, true));
            }

            if (File.Exists(_settings.OutputPath))
            {
                var outputFileName = Path.GetFileName(_settings.OutputPath);
                var baseScriptReferenceName = Path.GetFileName(_settings.GameDllPath);
                var baseScriptAssembly = Assembly.LoadFrom(_settings.GameDllPath);
                baseScriptAssembly.GetReferencedAssembliesAtPath(_settings.GameDllPath);
                var assembly = Assembly.LoadFrom(_settings.OutputPath);
                assembly.GetReferencedAssembliesAtPath(_settings.OutputPath);

                var scriptInterface = typeof(IScript);
                var assignableTypes = assembly.GetTypes().Where(x => x.IsAssignableTo(scriptInterface));
                var builtInTypes = baseScriptAssembly.GetTypes().Where(x => x.IsAssignableTo(scriptInterface));

                foreach (var type in assignableTypes) 
                {
                    AddScriptReferenceCheckedModel(currentReferences, type, outputFileName);
                }
                foreach (var type in builtInTypes)
                {
                    AddScriptReferenceCheckedModel(currentReferences, type, baseScriptReferenceName);
                }
            }

            DataContext = this;
        }

        private void AddScriptReferenceCheckedModel(List<ScriptReference> currentReferences, Type type, string dllPath) 
        {
            var scriptReference = new ScriptReference(dllPath, type.FullName);
            if (currentReferences.Any(x => x == scriptReference))
            {
                return;
            }
            ScriptReferenceCheckedModels.Add(new ScriptReferenceCheckedModel(scriptReference, false));
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
