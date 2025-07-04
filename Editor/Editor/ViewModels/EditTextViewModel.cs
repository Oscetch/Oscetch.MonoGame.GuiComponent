using Editor.Extensions;
using Editor.Handlers;
using Microsoft.Xna.Framework;
using System;
using System.Windows.Input;

namespace Editor.ViewModels
{
    public class EditTextViewModel : ViewModel
    {
        private readonly EditorViewModel _editorViewModel;

        public bool CenterText
        {
            get => _editorViewModel.SelectedParameters?.CenterText ?? false;
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                _editorViewModel.SelectedParameters.CenterText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NotCenterText));
            }
        }

        public bool NotCenterText => !CenterText;

        public float TextX
        {
            get => _editorViewModel.SelectedParameters?.TextPosition.X ?? 0;
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                var y = _editorViewModel.SelectedParameters.TextPosition.Y;
                _editorViewModel.SelectedParameters.TextPosition = new Vector2(value, y);
                OnPropertyChanged();
            }
        }

        public float TextY
        {
            get => _editorViewModel.SelectedParameters?.TextPosition.Y ?? 0;
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                var x = _editorViewModel.SelectedParameters.TextPosition.X;
                _editorViewModel.SelectedParameters.TextPosition = new Vector2(x, value);
                OnPropertyChanged();
            }
        }

        public float Scale
        {
            get => _editorViewModel.SelectedParameters?.TextScale ?? 0;
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                _editorViewModel.SelectedParameters.TextScale = value;
                OnPropertyChanged();
            }
        }

        public float Rotation
        {
            get => _editorViewModel.SelectedParameters?.TextRotation ?? 0f;
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                _editorViewModel.SelectedParameters.TextRotation = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get => _editorViewModel.SelectedParameters?.Text ?? string.Empty;
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                _editorViewModel.SelectedParameters.Text = value;
                OnPropertyChanged();
            }
        }

        public System.Windows.Media.Color Color
        {
            get => (_editorViewModel.SelectedParameters?.TextColor ?? Microsoft.Xna.Framework.Color.White).ToWindows();
            set
            {
                if (_editorViewModel.SelectedParameters == null)
                {
                    return;
                }

                _editorViewModel.SelectedParameters.TextColor = value.ToXna();
                OnPropertyChanged();
            }
        }

        public string Font
        {
            get => _editorViewModel.SelectedParameters?.SpriteFont ?? string.Empty;
            set
            {
                if (_editorViewModel.SelectedParameters == null) return;
                _editorViewModel.SelectedParameters.SpriteFont = value;
                OnPropertyChanged();
            }
        }

        public ICommand LeftAlign { get; }
        public ICommand RightAlign { get; }
        public ICommand TopAlign { get; }
        public ICommand BottomAlign { get; }
        public ICommand ScaleToBounds { get; }

        public EditTextViewModel(EditorViewModel editor)
        {
            _editorViewModel = editor;

            LeftAlign = new CommandHandler(OnLeftAlignTextClicked);
            RightAlign = new CommandHandler(OnRightAlignTextClicked);
            TopAlign = new CommandHandler(OnTopAlignTextClicked);
            BottomAlign = new CommandHandler(OnBottomAlignTextClicked);
            ScaleToBounds = new CommandHandler(OnScaleTextToBounds);

            _editorViewModel.SelectedControlChanged += EditorViewModel_SelectedControlChanged;
            _editorViewModel.SelectedControlPositionUpdated += EditorViewModel_SelectedControlChanged;
            _editorViewModel.SelectedControlSizeUpdated += EditorViewModel_SelectedControlChanged;
        }

        private void EditorViewModel_SelectedControlChanged(object sender, System.EventArgs e)
        {
            SyncWithEditor();
        }

        private void SyncWithEditor()
        {
            OnPropertyChanged(nameof(CenterText));
            OnPropertyChanged(nameof(NotCenterText));
            OnPropertyChanged(nameof(TextX));
            OnPropertyChanged(nameof(TextY));
            OnPropertyChanged(nameof(Text));
            OnPropertyChanged(nameof(Scale));
            OnPropertyChanged(nameof(Rotation));
            OnPropertyChanged(nameof(Color));
            OnPropertyChanged(nameof(Font));
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
            OnPropertyChanged(nameof(Scale));
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
            OnPropertyChanged(nameof(TextY));
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
            OnPropertyChanged(nameof(TextY));
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
            OnPropertyChanged(nameof(TextX));
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
            OnPropertyChanged(nameof(TextX));
        }
    }
}
