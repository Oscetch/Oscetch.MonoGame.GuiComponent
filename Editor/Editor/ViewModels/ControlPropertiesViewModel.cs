using Editor.Handlers;
using Editor.Modals;
using Microsoft.Xna.Framework;
using System;
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
            get => _editorViewModel.SelectedParameters?.BackgroundTexture2DPath ?? string.Empty;
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                var extension = Path.GetExtension(value);
                if (extension.Equals("xnb", StringComparison.CurrentCultureIgnoreCase))
                {
                    _editorViewModel.SelectedParameters.BackgroundTexture2DPath = Path.GetFileNameWithoutExtension(value);
                }
                else
                {
                    _editorViewModel.SelectedParameters.BackgroundTexture2DPath = value;
                }
                OnPropertyChanged();
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

        public ICommand EditScriptsCommand { get; }

        public ControlPropertiesViewModel(EditorViewModel editorViewModel)
        {
            _editorViewModel = editorViewModel;

            SyncEditor();

            _editorViewModel.SelectedControlChanged += EditorViewModel_SelectedControlChanged;
            _editorViewModel.SelectedControlPositionUpdated += EditorViewModel_SelectedControlPositionUpdated;
            _editorViewModel.SelectedControlSizeUpdated += EditorViewModel_SelectedControlSizeUpdated;

            EditScriptsCommand = new CommandHandler(OnEditScriptsClicked);
        }

        private void OnEditScriptsClicked()
        {
            if(_editorViewModel.SelectedParameters == null)
            {
                return;
            }

            var dialog = new ScriptReferenceDialog(_editorViewModel.SelectedParameters.ControlScripts);
            var result = dialog.ShowDialog() ?? false;
            if (result)
            {
                _editorViewModel.SelectedParameters.ControlScripts.Clear();
                foreach(var reference in dialog.ScriptReferenceCheckedModels)
                {
                    if (!reference.IsSelected)
                    {
                        continue;
                    }

                    _editorViewModel.SelectedParameters.ControlScripts.Add(reference.ScriptReference);
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
