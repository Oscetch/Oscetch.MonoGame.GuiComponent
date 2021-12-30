using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.ViewModels
{
    public class LeftPanelViewModel : ViewModel
    {
        private readonly EditorViewModel _editorViewModel;

        public TextPropertyControlViewModel NameViewModel { get; } = new TextPropertyControlViewModel
        {
            Title = "Name",
            Description = "The name of this GUI compoenent",
            OnChangedAction = name =>
            {
            }
        };

        public TextPropertyControlViewModel TextViewModel { get; } = new TextPropertyControlViewModel
        {
            Title = "Text",
            Description = "The text displayed in this control"
        };

        public Vector2PropertyControlViewModel PositionViewModel { get; } = new Vector2PropertyControlViewModel
        {
            Title = "Position",
            Description = "The position of the current control"
        };

        public Vector2PropertyControlViewModel SizeViewModel { get; } = new Vector2PropertyControlViewModel
        {
            Title = "Size",
            Description = "The size of the current control"
        };

        public Vector2PropertyControlViewModel TextPositionViewModel { get; } = new Vector2PropertyControlViewModel
        {
            Title = "Text Position",
            Description = "The position of the text related to this control"
        };

        public FloatPropertyControlViewModel TextScaleViewModel { get; } = new FloatPropertyControlViewModel
        {
            Title = "Text Scale",
            Description = "The scale of the text related to this control"
        };

        public FloatPropertyControlViewModel TextRotationViewModel { get; } = new FloatPropertyControlViewModel
        {
            Title = "Text Rotation",
            Description = "The rotation of the text related to this control"
        };

        public ColorPropertyControlViewModel TextColorViewModel { get; } = new ColorPropertyControlViewModel
        {
            Title = "Text Color",
            Description = "The color of the text related to this control"
        };

        public BooleanPropertyControlViewModel ShouldCenterTextViewModel { get; } = new BooleanPropertyControlViewModel
        {
            Title = "Should Center Text",
            Description = "Check if you want the text related to this control to be centered within it"
        };

        public TextPropertyControlViewModel SpriteFontViewModel { get; } = new TextPropertyControlViewModel
        {
            Title = "Sprite Font",
            Description = "The path to the font you want to be used for the text related to this control"
        };

        public BooleanPropertyControlViewModel HasBorderViewModel { get; } = new BooleanPropertyControlViewModel
        {
            Title = "Has Border",
            Description = "Check if you want this control to have a border"
        };

        public ColorPropertyControlViewModel BorderColorViewModel { get; } = new ColorPropertyControlViewModel
        {
            Title = "Border Color",
            Description = "The color of the border around this control"
        };

        public BooleanPropertyControlViewModel IsModalViewModel { get; } = new BooleanPropertyControlViewModel
        {
            Title = "Is Modal",
            Description = "Makes it so that this control is the only visible and enabled modal control. " +
            "Other non-modal controls can not be interacted with when this control is enabled and visible"
        };

        public LeftPanelViewModel(EditorViewModel editorViewModel)
        {
            _editorViewModel = editorViewModel;

            MapEditorToProperties();

            _editorViewModel.SelectedControlChanged += EditorViewModel_SelectedControlChanged;
            _editorViewModel.SelectedControlPositionUpdated += EditorViewModel_SelectedControlPositionUpdated;
            _editorViewModel.SelectedControlSizeUpdated += EditorViewModel_SelectedControlSizeUpdated;
        }

        private void EditorViewModel_SelectedControlSizeUpdated(object sender, EventArgs e)
        {
            SizeViewModel.PropertyValue = _editorViewModel.SelectedParameters.Size;
        }

        private void EditorViewModel_SelectedControlPositionUpdated(object sender, EventArgs e)
        {
            PositionViewModel.PropertyValue = _editorViewModel.SelectedParameters.Position;
            TextPositionViewModel.PropertyValue = _editorViewModel.SelectedParameters.Position;
        }

        private System.Windows.Media.Color XnaColorToMediaColor(Color color)
        {
            return new System.Windows.Media.Color
            {
                R = color.R,
                G = color.G,
                B = color.B,
                A = color.A
            };
        }

        private void MapEditorToProperties()
        {
            var parameters = _editorViewModel.SelectedParameters;

            if(parameters == null)
            {
                NameViewModel.PropertyValue = string.Empty;
                TextViewModel.PropertyValue = string.Empty;
                PositionViewModel.PropertyValue = Vector2.Zero;
                SizeViewModel.PropertyValue = Vector2.Zero;
                TextPositionViewModel.PropertyValue = Vector2.Zero;
                TextScaleViewModel.PropertyValue = 1f;
                TextRotationViewModel.PropertyValue = 0f;
                TextColorViewModel.PropertyValue = new System.Windows.Media.Color();
                ShouldCenterTextViewModel.PropertyValue = false;
                SpriteFontViewModel.PropertyValue = "Fonts/DefaultFont.xnb";
                HasBorderViewModel.PropertyValue = false;
                BorderColorViewModel.PropertyValue = new System.Windows.Media.Color();
                IsModalViewModel.PropertyValue = false;
                return;
            }

            NameViewModel.PropertyValue = parameters.Name;
            TextViewModel.PropertyValue = parameters.Text;
            PositionViewModel.PropertyValue = parameters.Position;
            SizeViewModel.PropertyValue = parameters.Size;
            TextPositionViewModel.PropertyValue = parameters.TextPosition;
            TextScaleViewModel.PropertyValue = parameters.TextScale;
            TextRotationViewModel.PropertyValue = parameters.TextRotation;
            TextColorViewModel.PropertyValue = XnaColorToMediaColor(parameters.TextColor);
            ShouldCenterTextViewModel.PropertyValue = parameters.CenterText;
            SpriteFontViewModel.PropertyValue = $"{parameters.SpriteFont}.xnb";
            HasBorderViewModel.PropertyValue = parameters.HasBorder;
            BorderColorViewModel.PropertyValue = XnaColorToMediaColor(parameters.BorderColor);
            IsModalViewModel.PropertyValue = parameters.IsModal;
        }

        private void EditorViewModel_SelectedControlChanged(object sender, EventArgs e)
        {
            MapEditorToProperties();
        }
    }
}
