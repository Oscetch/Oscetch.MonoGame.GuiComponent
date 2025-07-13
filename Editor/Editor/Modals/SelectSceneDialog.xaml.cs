using Editor.ViewModels;
using Oscetch.ScriptComponent;
using System.Windows;

namespace Editor.Modals
{
    /// <summary>
    /// Interaction logic for SelectSceneDialog.xaml
    /// </summary>
    public partial class SelectSceneDialog : Window
    {
        public SelectSceneDialog()
        {
            InitializeComponent();
            DataContext = new SelectSceneDialogViewModel();
        }

        public ScriptReference Selected => DataContext is SelectSceneDialogViewModel viewModel ? viewModel.Selected : null;
    }
}
