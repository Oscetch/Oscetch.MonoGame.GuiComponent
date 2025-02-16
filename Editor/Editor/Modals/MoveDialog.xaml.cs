using Editor.Models;
using Oscetch.MonoGame.GuiComponent;
using Oscetch.MonoGame.GuiComponent.Interfaces;
using Oscetch.MonoGame.GuiComponent.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Editor.Modals
{
    /// <summary>
    /// Interaction logic for MoveDialog.xaml
    /// </summary>
    public partial class MoveDialog : Window
    {
        public ObservableCollection<SelectableControlModel> SelectableParametersModels { get; set; }
        public SelectableControlModel Selected { get; set; }
        public MoveDialog(IEnumerable<GuiControl<IGameToGuiService>> parameters)
        {
            InitializeComponent();
            SelectableParametersModels = [];
            foreach (var p in parameters)
            {
                SelectableParametersModels.Add(new SelectableControlModel(p));
            }
            DataContext = this;
        }

        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
