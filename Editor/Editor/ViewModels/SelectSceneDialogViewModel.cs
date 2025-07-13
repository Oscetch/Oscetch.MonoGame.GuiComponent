using Editor.Extensions;
using Editor.Models;
using Oscetch.MonoGame.GuiComponent.Interfaces;
using Oscetch.ScriptComponent;
using Oscetch.ScriptComponent.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Editor.ViewModels
{
    public class SelectSceneDialogViewModel : ViewModel
    {
        private readonly ProjectSettings _settings;
        public ObservableCollection<SelectSceneModel> Scenes { get; } = [];
        public ScriptReference Selected { get; private set; }

        public SelectSceneDialogViewModel()
        {
            _settings = ProjectSettings.GetSettings();
            var scriptInterface = typeof(IScript);
            var sceneInterface = typeof(IGuiScene);
            if (File.Exists(_settings.GameDllPath))
            {
                var baseScriptReferenceName = Path.GetFileName(_settings.GameDllPath);
                var baseScriptAssembly = Assembly.LoadFrom(_settings.GameDllPath);
                baseScriptAssembly.GetReferencedAssembliesAtPath(_settings.GameDllPath);

                try
                {
                    var builtInTypes = baseScriptAssembly.GetTypes()
                        .Where(x => x.IsAssignableTo(scriptInterface))
                        .Where(x => x.GetInterfaces().Any(y => y.Name == sceneInterface.Name));


                    foreach (var type in builtInTypes)
                    {
                        var scriptReference = new ScriptReference(_settings.GameDllPath, type.FullName);
                        var selectSceneModel = new SelectSceneModel(scriptReference, window =>
                        {
                            Selected = scriptReference;
                            window.DialogResult = true;
                        });
                        Scenes.Add(selectSceneModel);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
    }
}
