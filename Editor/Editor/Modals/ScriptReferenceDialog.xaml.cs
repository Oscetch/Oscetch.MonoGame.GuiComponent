using Editor.Extensions;
using Editor.Models;
using Oscetch.MonoGame.GuiComponent.Interfaces;
using Oscetch.ScriptComponent;
using Oscetch.ScriptComponent.Interfaces;
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
        private readonly Settings _settings;

        public ObservableCollection<ScriptReferenceCheckedModel> ScriptReferenceCheckedModels { get; set; }

        public ScriptReferenceDialog(List<ScriptReference> currentReferences)
        {
            _settings = Settings.GetSettings();
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
                var baseScriptAssembly = Assembly.LoadFrom(_settings.BaseScriptReference.DllPath);
                baseScriptAssembly.GetReferencedAssembliesAtPath(_settings.BaseScriptReference.DllPath);
                var assembly = Assembly.LoadFrom(_settings.OutputPath);
                assembly.GetReferencedAssembliesAtPath(_settings.OutputPath);

                var scriptInterface = typeof(IGuiScript<>);
                var assignableTypes = assembly.GetTypes().Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == scriptInterface)).ToList();

                foreach (var type in assignableTypes) 
                {
                    var scriptReference = new ScriptReference(outputFileName, type.FullName);
                    if(currentReferences.Any(x => x == scriptReference))
                    {
                        continue;
                    }
                    ScriptReferenceCheckedModels.Add(new ScriptReferenceCheckedModel(scriptReference, false));
                }
            }

            DataContext = this;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
