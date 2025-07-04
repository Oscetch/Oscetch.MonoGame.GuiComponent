using Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Editor.Modals
{
    /// <summary>
    /// Interaction logic for EditProjectSettingsDialog.xaml
    /// </summary>
    public partial class EditProjectSettingsDialog : Window
    {
        public EditProjectSettingsDialog(string dialogTitle, EditProjectSettingsDialogViewModel viewModel)
        {
            InitializeComponent();

            Title = dialogTitle;

            DataContext = viewModel;
        }
    }
}
