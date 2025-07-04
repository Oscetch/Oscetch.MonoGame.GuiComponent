namespace Editor.ViewModels
{
    public class LeftPanelViewModel : ViewModel
    {
        private ControlPropertiesViewModel _controlPropertiesViewModel;

        public ControlPropertiesViewModel ControlPropertiesViewModel
        {
            get => _controlPropertiesViewModel;
            set
            {
                _controlPropertiesViewModel = value;
                OnPropertyChanged();
            }
        }

        public LeftPanelViewModel(EditorViewModel editorViewModel)
        {
            ControlPropertiesViewModel = new ControlPropertiesViewModel(editorViewModel);
        }
    }
}
