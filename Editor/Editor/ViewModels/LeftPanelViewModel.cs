namespace Editor.ViewModels
{
    public class LeftPanelViewModel : ViewModel
    {
        private ControlPropertiesViewModel _controlPropertiesViewModel;
        private TopLeftViewModel _topLeftViewModel;

        public ControlPropertiesViewModel ControlPropertiesViewModel
        {
            get => _controlPropertiesViewModel;
            set
            {
                _controlPropertiesViewModel = value;
                OnPropertyChanged();
            }
        }

        public TopLeftViewModel TopLeftViewModel
        {
            get => _topLeftViewModel;
            set
            {
                _topLeftViewModel = value;
                OnPropertyChanged();
            }
        }

        public LeftPanelViewModel(EditorViewModel editorViewModel)
        {
            ControlPropertiesViewModel = new ControlPropertiesViewModel(editorViewModel);
            TopLeftViewModel = new TopLeftViewModel(editorViewModel);
        }
    }
}
