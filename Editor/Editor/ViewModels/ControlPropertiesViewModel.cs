using Editor.Extensions;
using Editor.Handlers;
using Editor.Modals;
using Editor.Models;
using Microsoft.Xna.Framework;
using Oscetch.MonoGame.GuiComponent.Models;
using Oscetch.MonoGame.Textures;
using Oscetch.MonoGame.Textures.Enums;
using Oscetch.ScriptComponent;
using Oscetch.ScriptComponent.Attributes;
using Oscetch.ScriptComponent.Interfaces;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace Editor.ViewModels
{
    public class ControlPropertiesViewModel : ViewModel
    {
        private readonly EditorViewModel _editorViewModel;
        private ObservableCollection<ScriptValueParameterModel> _scriptValues;

        public string GuiControlName
        {
            get => _editorViewModel.SelectedParameters?.Name;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.Name = value;
                OnPropertyChanged();
            }
        }

        public bool GuiControlIsEnabled
        {
            get => _editorViewModel.SelectedParameters?.IsEnabled ?? false;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.IsEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool GuiControlIsVisible
        {
            get => _editorViewModel.SelectedParameters?.IsVisible ?? false;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.IsVisible = value;
                OnPropertyChanged();
            }
        }

        public string GuiControlBackground
        {
            get => _editorViewModel.SelectedParameters?.Background?.Path ?? string.Empty;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                if (value == null)
                {
                    _editorViewModel.SelectedParameters.Background = null;
                    _editorViewModel.LoadSelected();
                }
                else
                {
                    var extension = Path.GetExtension(value);
                    if (extension.Equals(".xnb", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var settings = ProjectSettings.GetSettings();
                        var relativePath = Path.GetRelativePath(settings.ContentBinPath, value);
                        var fileName = Path.GetFileNameWithoutExtension(value);
                        var parts = relativePath.Split(Path.DirectorySeparatorChar);
                        parts[^1] = fileName;
                        var finalPath = string.Join(Path.DirectorySeparatorChar, parts);
                        _editorViewModel.SelectedParameters.Background = finalPath;
                        _editorViewModel.LoadSelected();
                    }
                }
                OnPropertyChanged();
                OnCustomTextureChanged();
            }
        }

        public int CustomTextureWidth
        {
            get => _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.Size.X ?? 0;
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                var parameters = 
                    _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.ToBuilder()
                    ?? new CustomTextureParameters.CustomTextureParametersBuilder();
                _editorViewModel.SelectedParameters.Background = parameters.WithWidth(value).Build();
                _editorViewModel.LoadSelected();
                OnPropertyChanged();
                OnPropertyChanged(nameof(GuiControlBackground));
            }
        }

        public int CustomTextureHeight
        {
            get => _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.Size.Y ?? 0;
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                var parameters =
                    _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.ToBuilder()
                    ?? new CustomTextureParameters.CustomTextureParametersBuilder();
                _editorViewModel.SelectedParameters.Background = parameters.WithHeight(value).Build();
                _editorViewModel.LoadSelected();
                OnPropertyChanged();
                OnPropertyChanged(nameof(GuiControlBackground));
            }
        }

        public ShapeType CustomTextureShapeType
        {
            get => _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.ShapeType ?? ShapeType.RectangleCornerRadius;
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                var parameters =
                    _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.ToBuilder()
                    ?? new CustomTextureParameters.CustomTextureParametersBuilder();
                _editorViewModel.SelectedParameters.Background = parameters.WithShape(value).Build();
                _editorViewModel.LoadSelected();
                OnPropertyChanged();
                OnPropertyChanged(nameof(GuiControlBackground));
            }
        }

        public System.Windows.Media.Color CustomTextureColor
        {
            get => (_editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.FillColor ?? Color.Transparent).ToWindows();
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                var parameters =
                    _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.ToBuilder()
                    ?? new CustomTextureParameters.CustomTextureParametersBuilder();
                _editorViewModel.SelectedParameters.Background = parameters.WithFillColor(value.ToXna()).Build();
                _editorViewModel.LoadSelected();
                OnPropertyChanged();
                OnPropertyChanged(nameof(GuiControlBackground));
            }
        }

        public int CustomTextureCornerRadius
        {
            get => _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.CornerRadius ?? 0;
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                var parameters =
                    _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.ToBuilder()
                    ?? new CustomTextureParameters.CustomTextureParametersBuilder();
                _editorViewModel.SelectedParameters.Background = parameters.WithCornerRadius(value).Build();
                _editorViewModel.LoadSelected();
                OnPropertyChanged();
                OnPropertyChanged(nameof(GuiControlBackground));
            }
        }

        public int CustomTextureBorderThickness
        {
            get => _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.BorderThickness ?? 0;
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                var parameters =
                    _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.ToBuilder()
                    ?? new CustomTextureParameters.CustomTextureParametersBuilder();
                _editorViewModel.SelectedParameters.Background = parameters.WithBorderThickness(value).Build();
                _editorViewModel.LoadSelected();
                OnPropertyChanged();
                OnPropertyChanged(nameof(GuiControlBackground));
            }
        }

        public System.Windows.Media.Color CustomTextureBorder
        {
            get => (_editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.BorderColor ?? Color.Transparent).ToWindows();
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                var parameters =
                    _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.ToBuilder()
                    ?? new CustomTextureParameters.CustomTextureParametersBuilder();
                _editorViewModel.SelectedParameters.Background = parameters.WithBorderColor(value.ToXna()).Build();
                _editorViewModel.LoadSelected();
                OnPropertyChanged();
                OnPropertyChanged(nameof(GuiControlBackground));
            }
        }

        public ObservableCollection<ScriptValueParameterModel> ScriptParameters
        {
            get => _scriptValues;
            set 
            {
                _scriptValues = value;
                OnPropertyChanged();
            }
        }

        public ICommand SetCustomTextureSizeToControlSize { get; }

        public float GuiControlPositionX
        {
            get => _editorViewModel.SelectedParameters?.Position.X ?? 0f;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.Position =
                    new Vector2(value, _editorViewModel.SelectedParameters.Position.Y);
                OnPropertyChanged();
            }
        }

        public float GuiControlPositionY
        {
            get => _editorViewModel.SelectedParameters?.Position.Y ?? 0f;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.Position =
                    new Vector2(_editorViewModel.SelectedParameters.Position.X, value);
                OnPropertyChanged();
            }
        }

        public float GuiControlSizeX
        {
            get => _editorViewModel.SelectedParameters?.Size.X ?? 0f;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.Size =
                    new Vector2(value, _editorViewModel.SelectedParameters.Size.Y);
                OnPropertyChanged();
            }
        }

        public float GuiControlSizeY
        {
            get => _editorViewModel.SelectedParameters?.Size.Y ?? 0f;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.Size =
                    new Vector2(_editorViewModel.SelectedParameters.Size.X, value);
                OnPropertyChanged();
            }
        }

        public bool GuiControlHasBorder
        {
            get => _editorViewModel.SelectedParameters?.HasBorder ?? false;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.HasBorder = value;
                OnPropertyChanged();
            }
        }

        public System.Windows.Media.Color GuiControlBorderColor
        {
            get => (_editorViewModel.SelectedParameters?.BorderColor ?? Color.White).ToWindows();
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.BorderColor = value.ToXna();
                OnPropertyChanged();
            }
        }

        public bool GuiControlIsModal
        {
            get => _editorViewModel.SelectedParameters?.IsModal ?? false;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.IsModal = value;
                OnPropertyChanged();
            }
        }

        public bool GuiControlClip
        {
            get => _editorViewModel.SelectedParameters?.Clip ?? false;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.Clip = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditScriptsCommand { get; }

        public ControlPropertiesViewModel(EditorViewModel editorViewModel)
        {
            _editorViewModel = editorViewModel;

            SyncEditor();

            _editorViewModel.SelectedControlChanged += EditorViewModel_SelectedControlChanged;
            _editorViewModel.SelectedControlPositionUpdated += EditorViewModel_SelectedControlPositionUpdated;
            _editorViewModel.SelectedControlSizeUpdated += EditorViewModel_SelectedControlSizeUpdated;

            EditScriptsCommand = new CommandHandler(OnEditScriptsClicked);
            SetCustomTextureSizeToControlSize = new CommandHandler(OnSetCustomTextureSizeToControlSize);
        }

        private void OnSetCustomTextureSizeToControlSize()
        {
            var p = _editorViewModel.SelectedParameters;
            if (p == null) return;
            CustomTextureWidth = (int)p.Size.X;
            CustomTextureHeight = (int)p.Size.Y;
        }

        private void OnEditScriptsClicked()
        {
            if(_editorViewModel.SelectedParameters == null)
            {
                return;
            }
            var settings = ProjectSettings.GetSettings();
            var allScripts = new List<ScriptReference>();
            allScripts.AddRange(_editorViewModel.SelectedParameters.ControlScripts);
            allScripts.AddRange(_editorViewModel.SelectedParameters.BuiltInScripts);

            var dialog = new ScriptReferenceDialog(allScripts);
            var result = dialog.ShowDialog() ?? false;
            if (result)
            {
                _editorViewModel.SelectedParameters.ControlScripts.Clear();
                _editorViewModel.SelectedParameters.BuiltInScripts.Clear();
                foreach (var reference in dialog.ScriptReferenceCheckedModels)
                {
                    if (!reference.IsSelected)
                    {
                        continue;
                    }
                    if (settings.GameDllPath.EndsWith(reference.ScriptReference.DllPath))
                    {
                        _editorViewModel.SelectedParameters.BuiltInScripts.Add(reference.ScriptReference);
                    }
                    else
                    {
                        _editorViewModel.SelectedParameters.ControlScripts.Add(reference.ScriptReference);
                    }
                }
                OnScriptParametersChanged();
            }
        }

        private void SyncEditor()
        {
            OnPropertyChanged(nameof(GuiControlName));
            OnPropertyChanged(nameof(GuiControlIsEnabled));
            OnPropertyChanged(nameof(GuiControlIsVisible));
            OnPropertyChanged(nameof(GuiControlBackground));
            OnPropertyChanged(nameof(GuiControlPositionX));
            OnPropertyChanged(nameof(GuiControlSizeX));
            OnPropertyChanged(nameof(GuiControlPositionY));
            OnPropertyChanged(nameof(GuiControlSizeY));
            OnPropertyChanged(nameof(GuiControlHasBorder));
            OnPropertyChanged(nameof(GuiControlBorderColor));
            OnPropertyChanged(nameof(GuiControlIsModal));
            OnPropertyChanged(nameof(GuiControlClip));
            OnCustomTextureChanged();
            OnScriptParametersChanged();
        }

        private void OnScriptParametersChanged()
        {
            if (_editorViewModel.SelectedParameters == null)
            {
                ScriptParameters = [];
            }
            else
            {
                ScriptParameters = new ObservableCollection<ScriptValueParameterModel>(GetScriptParameters(_editorViewModel.SelectedParameters));
            }
        }

        private void OnCustomTextureChanged()
        {
            OnPropertyChanged(nameof(CustomTextureHeight));
            OnPropertyChanged(nameof(CustomTextureWidth));
            OnPropertyChanged(nameof(CustomTextureShapeType));
            OnPropertyChanged(nameof(CustomTextureColor));
            OnPropertyChanged(nameof(CustomTextureCornerRadius));
            OnPropertyChanged(nameof(CustomTextureBorderThickness));
            OnPropertyChanged(nameof(CustomTextureBorder));
        }

        private void EditorViewModel_SelectedControlSizeUpdated(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(GuiControlSizeX));
            OnPropertyChanged(nameof(GuiControlSizeY));
        }

        private void EditorViewModel_SelectedControlPositionUpdated(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(GuiControlPositionX));
            OnPropertyChanged(nameof(GuiControlPositionY));
        }

        private void EditorViewModel_SelectedControlChanged(object sender, EventArgs e)
        {
            SyncEditor();
        }

        private static IEnumerable<ScriptValueParameterModel> GetScriptParameters(GuiControlParameters parameters)
        {
            var settings = ProjectSettings.GetSettings();

            if (!File.Exists(settings.OutputPath)) yield break;
            var baseScriptReferenceName = Path.GetFileName(settings.GameDllPath);
            var baseScriptAssembly = Assembly.LoadFrom(settings.GameDllPath);
            baseScriptAssembly.GetReferencedAssembliesAtPath(settings.GameDllPath);
            var assembly = Assembly.LoadFrom(settings.OutputPath);
            assembly.GetReferencedAssembliesAtPath(settings.OutputPath);

            var scriptInterface = typeof(IScript);
            var assignableTypes = assembly.GetTypes().Where(x => x.IsAssignableTo(scriptInterface));
            var builtInTypes = baseScriptAssembly.GetTypes().Where(x => x.IsAssignableTo(scriptInterface));

            foreach (var reference in parameters.BuiltInScripts)
            {
                var type = baseScriptAssembly.GetType(reference.ScriptClassName);
                var dict = reference.Params.ToDictionary(x => x.Name);
                foreach (var scriptValue in GetScriptParameters(type, dict))
                {
                    yield return new ScriptValueParameterModel(reference, scriptValue);
                }
            }
            foreach (var reference in parameters.ControlScripts)
            {
                var type = assembly.GetType(reference.ScriptClassName);
                var dict = reference.Params.ToDictionary(x => x.Name);
                foreach (var scriptValue in GetScriptParameters(type, dict))
                {
                    yield return new ScriptValueParameterModel(reference, scriptValue);
                }
            }
        }

        private static IEnumerable<ScriptValueParameter> GetScriptParameters(Type type, Dictionary<string, ScriptValueParameter> dict)
        {
            foreach (var field in type.GetFields())
            {
                foreach (var scriptParameter in field.GetCustomAttributes<ScriptParameter>())
                {
                    if (dict.TryGetValue(scriptParameter.Name, out var existingParam)) yield return existingParam;
                    else yield return new ScriptValueParameter(scriptParameter.Name, "");
                }
            }
            foreach (var property in type.GetProperties())
            {
                foreach (var scriptParameter in property.GetCustomAttributes<ScriptParameter>())
                {
                    if (dict.TryGetValue(scriptParameter.Name, out var existingParam)) yield return existingParam;
                    else yield return new ScriptValueParameter(scriptParameter.Name, "");
                }
            }
        }
    }
}
