using Editor.Extensions;
using Editor.Handlers;
using Editor.Modals;
using Microsoft.Xna.Framework;
using Oscetch.MonoGame.Textures;
using Oscetch.MonoGame.Textures.Enums;
using Oscetch.ScriptComponent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace Editor.ViewModels
{
    public class ControlPropertiesViewModel : ViewModel
    {
        private readonly EditorViewModel _editorViewModel;

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
                        var settings = Settings.GetSettings();
                        var relativePath = Path.GetRelativePath(settings.ContentPath, value);
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
            get => XnaColorToMediaColor(_editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.FillColor ?? Color.Transparent);
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                var parameters =
                    _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.ToBuilder()
                    ?? new CustomTextureParameters.CustomTextureParametersBuilder();
                _editorViewModel.SelectedParameters.Background = parameters.WithFillColor(MediaColorToXnaColor(value)).Build();
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
            get => XnaColorToMediaColor(_editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.BorderColor ?? Color.Transparent);
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                var parameters =
                    _editorViewModel.SelectedParameters?.Background?.CustomTextureParameters?.ToBuilder()
                    ?? new CustomTextureParameters.CustomTextureParametersBuilder();
                _editorViewModel.SelectedParameters.Background = parameters.WithBorderColor(MediaColorToXnaColor(value)).Build();
                _editorViewModel.LoadSelected();
                OnPropertyChanged();
                OnPropertyChanged(nameof(GuiControlBackground));
            }
        }

        public string GuiControlText
        {
            get => _editorViewModel.SelectedParameters?.Text;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.Text = value;
                OnPropertyChanged();
            }
        }

        public ICommand LeftAlignTextCommand { get; }
        public ICommand TopAlignTextCommand { get; }
        public ICommand RightAlignTextCommand { get; }
        public ICommand BottomAlignTextCommand { get; }
        public ICommand ScaleTextToBoundsCommand { get; }
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

        public float GuiControlTextPositionX
        {
            get => _editorViewModel.SelectedParameters?.TextPosition.X ?? 0f;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.TextPosition =
                    new Vector2(value, _editorViewModel.SelectedParameters.TextPosition.Y);
                OnPropertyChanged();
            }
        }

        public float GuiControlTextPositionY
        {
            get => _editorViewModel.SelectedParameters?.TextPosition.Y ?? 0f;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.TextPosition =
                    new Vector2(_editorViewModel.SelectedParameters.TextPosition.X, value);
                OnPropertyChanged();
            }
        }

        public float GuiControlTextScale
        {
            get => _editorViewModel.SelectedParameters?.TextScale ?? 1f;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.TextScale = value;
                OnPropertyChanged();
            }
        }

        public float GuiControlTextRotation
        {
            get => _editorViewModel.SelectedParameters?.TextRotation ?? 0f;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.TextRotation = value;
                OnPropertyChanged();
            }
        }

        public System.Windows.Media.Color GuiControlTextColor
        {
            get => XnaColorToMediaColor(_editorViewModel.SelectedParameters?.TextColor ?? Color.White);
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.TextColor = MediaColorToXnaColor(value);
                OnPropertyChanged();
            }
        }

        public bool GuiControlShouldCenterText
        {
            get => _editorViewModel.SelectedParameters?.CenterText ?? false;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.CenterText = value;
                OnPropertyChanged();
            }
        }

        public string GuiControlSpriteFont
        {
            get => _editorViewModel.SelectedParameters?.SpriteFont ?? string.Empty;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.SpriteFont = Path.GetFileNameWithoutExtension(value);
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
            get => XnaColorToMediaColor(_editorViewModel.SelectedParameters?.BorderColor ?? Color.White);
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.BorderColor = MediaColorToXnaColor(value);
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
            LeftAlignTextCommand = new CommandHandler(OnLeftAlignTextClicked);
            RightAlignTextCommand = new CommandHandler(OnRightAlignTextClicked);
            TopAlignTextCommand = new CommandHandler(OnTopAlignTextClicked);
            BottomAlignTextCommand = new CommandHandler(OnBottomAlignTextClicked);
            ScaleTextToBoundsCommand = new CommandHandler(OnScaleTextToBounds);
            SetCustomTextureSizeToControlSize = new CommandHandler(OnSetCustomTextureSizeToControlSize);
        }

        private void OnSetCustomTextureSizeToControlSize()
        {
            var p = _editorViewModel.SelectedParameters;
            if (p == null) return;
            CustomTextureWidth = (int)p.Size.X;
            CustomTextureHeight = (int)p.Size.Y;
        }

        private void OnScaleTextToBounds() 
        {
            var p = _editorViewModel.SelectedParameters;
            if (p == null || string.IsNullOrEmpty(p.Text))
            {
                return;
            }
            var textSize = _editorViewModel.SelectedTextSizeWithoutScale();
            var widthFraction = p.Size.X / textSize.X;
            var heightFraction = p.Size.Y / textSize.Y;

            var smallestFraction = Math.Min(widthFraction, heightFraction);
            p.TextScale = smallestFraction;
            OnPropertyChanged(nameof(GuiControlTextScale));
        }

        private void OnBottomAlignTextClicked()
        {
            var p = _editorViewModel.SelectedParameters;
            if (p == null || string.IsNullOrEmpty(p.Text))
            {
                return;
            }
            var textSize = _editorViewModel.SelectedTextSizeWithoutScale() * p.TextScale;

            p.TextPosition = new Vector2(p.TextPosition.X, p.Position.Y + p.Size.Y - (textSize.Y / 2));
            OnPropertyChanged(nameof(GuiControlTextPositionY));
        }

        private void OnTopAlignTextClicked()
        {
            var p = _editorViewModel.SelectedParameters;
            if (p == null || string.IsNullOrEmpty(p.Text))
            {
                return;
            }
            var textSize = _editorViewModel.SelectedTextSizeWithoutScale() * p.TextScale;

            p.TextPosition = new Vector2(p.TextPosition.X, p.Position.Y + (textSize.Y / 2));
            OnPropertyChanged(nameof(GuiControlTextPositionY));
        }

        private void OnRightAlignTextClicked()
        {
            var p = _editorViewModel.SelectedParameters;
            if (p == null || string.IsNullOrEmpty(p.Text))
            {
                return;
            }
            var textSize = _editorViewModel.SelectedTextSizeWithoutScale() * p.TextScale;

            p.TextPosition = new Vector2(p.Position.X + p.Size.X - (textSize.X / 2), p.TextPosition.Y);
            OnPropertyChanged(nameof(GuiControlTextPositionX));
        }

        private void OnLeftAlignTextClicked() 
        {
            var p = _editorViewModel.SelectedParameters;
            if (p == null || string.IsNullOrEmpty(p.Text))
            {
                return;
            }
            var textSize = _editorViewModel.SelectedTextSizeWithoutScale() * p.TextScale;

            p.TextPosition = new Vector2(p.Position.X + (textSize.X / 2), p.TextPosition.Y);
            OnPropertyChanged(nameof(GuiControlTextPositionX));
        }

        private void OnEditScriptsClicked()
        {
            if(_editorViewModel.SelectedParameters == null)
            {
                return;
            }
            var settings = Settings.GetSettings();
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
                    if (settings.BaseScriptReference.DllPath.EndsWith(reference.ScriptReference.DllPath))
                    {
                        _editorViewModel.SelectedParameters.BuiltInScripts.Add(reference.ScriptReference);
                    }
                    else
                    {
                        _editorViewModel.SelectedParameters.ControlScripts.Add(reference.ScriptReference);
                    }
                }
            }
        }

        private static System.Windows.Media.Color XnaColorToMediaColor(Color color)
        {
            return new System.Windows.Media.Color
            {
                R = color.R,
                G = color.G,
                B = color.B,
                A = color.A
            };
        }

        private static Color MediaColorToXnaColor(System.Windows.Media.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        private void SyncEditor()
        {
            OnPropertyChanged(nameof(GuiControlName));
            OnPropertyChanged(nameof(GuiControlIsEnabled));
            OnPropertyChanged(nameof(GuiControlIsVisible));
            OnPropertyChanged(nameof(GuiControlBackground));
            OnPropertyChanged(nameof(GuiControlText));
            OnPropertyChanged(nameof(GuiControlPositionX));
            OnPropertyChanged(nameof(GuiControlSizeX));
            OnPropertyChanged(nameof(GuiControlTextPositionX));
            OnPropertyChanged(nameof(GuiControlPositionY));
            OnPropertyChanged(nameof(GuiControlSizeY));
            OnPropertyChanged(nameof(GuiControlTextPositionY));
            OnPropertyChanged(nameof(GuiControlTextScale));
            OnPropertyChanged(nameof(GuiControlTextRotation));
            OnPropertyChanged(nameof(GuiControlTextColor));
            OnPropertyChanged(nameof(GuiControlShouldCenterText));
            OnPropertyChanged(nameof(GuiControlSpriteFont));
            OnPropertyChanged(nameof(GuiControlHasBorder));
            OnPropertyChanged(nameof(GuiControlBorderColor));
            OnPropertyChanged(nameof(GuiControlIsModal));
            OnPropertyChanged(nameof(GuiControlClip));
            OnCustomTextureChanged();
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
            OnPropertyChanged(nameof(GuiControlTextPositionX));
            OnPropertyChanged(nameof(GuiControlPositionY));
            OnPropertyChanged(nameof(GuiControlTextPositionY));
        }

        private void EditorViewModel_SelectedControlChanged(object sender, EventArgs e)
        {
            SyncEditor();
        }
    }
}
