using Oscetch.MonoGame.GuiComponent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public EditorViewModel EditorViewModel { get; }
        public LeftPanelViewModel LeftPanelViewModel { get; }

        public MainWindowViewModel()
        {
            EditorViewModel = new EditorViewModel();
            LeftPanelViewModel = new LeftPanelViewModel(EditorViewModel);
        }
    }
}
