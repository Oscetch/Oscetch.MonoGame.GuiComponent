using Editor.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Editor.ViewModels
{
    public class ControlTreeViewModel : ViewModel
    {
        private readonly EditorViewModel _editorViewModel;

        public ObservableCollection<ControlTreeItem> RootItems { get; set; }

        public ControlTreeViewModel(EditorViewModel editorViewModel) 
        {
            _editorViewModel = editorViewModel;
            editorViewModel.OnReset += EditorViewModel_OnReset;
            EditorSettings.Load().ProjectChanged += ControlTreeViewModel_ProjectChanged;
        }

        private void ControlTreeViewModel_ProjectChanged(object sender, System.EventArgs e)
        {
            EditorViewModel_OnReset(sender, e);
        }

        private void EditorViewModel_OnReset(object sender, System.EventArgs e)
        {
            if (_editorViewModel.Children == null) return;
            var items = _editorViewModel.Children.Select(x => new ControlTreeItem(x, id => _editorViewModel.SetSelectedChild(id))).ToList();
            RootItems = new ObservableCollection<ControlTreeItem>(items);
            OnPropertyChanged(nameof(RootItems));
        }
    }
}
