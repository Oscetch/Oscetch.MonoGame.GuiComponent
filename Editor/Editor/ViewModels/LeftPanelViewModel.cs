using Editor.Handlers;
using Microsoft.Xna.Framework;
using Oscetch.MonoGame.GuiComponent.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Editor.ViewModels
{
    public class LeftPanelViewModel : ViewModel
    {
        private readonly EditorViewModel _editorViewModel;

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
            _editorViewModel = editorViewModel;
            ControlPropertiesViewModel = new ControlPropertiesViewModel(editorViewModel);
            TopLeftViewModel = new TopLeftViewModel(editorViewModel);
        }
    }
}
