using Editor.ViewModels;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for FilePropertyControl.xaml
    /// </summary>
    public partial class FilePropertyControl : UserControl
    {
        public FilePropertyControl()
        {
            InitializeComponent();
        }

        public string FileFilter 
        { 
            get => (string)GetValue(FileFilterProperty); 
            set => SetValue(FileFilterProperty, value);
        }

        public TextPropertyControlViewModel TextControlViewModel
        {
            get => (TextPropertyControlViewModel)GetValue(TextControlViewModelProperty);
            set => SetValue(TextControlViewModelProperty, value);
        }


        public static readonly DependencyProperty FileFilterProperty =
            DependencyProperty.Register(nameof(FileFilter), typeof(string), typeof(FilePropertyControl));

        public static readonly DependencyProperty TextControlViewModelProperty = DependencyProperty.Register(nameof(TextControlViewModel),
            typeof(TextPropertyControlViewModel), typeof(FilePropertyControl), new PropertyMetadata(OnViewModelChanged));

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FilePropertyControl view || e.NewValue is not TextPropertyControlViewModel viewModel)
            {
                return;
            }

            view.DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog 
            {
                Filter = FileFilter
            };

            var result = fileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                FileTextBox.Text = fileDialog.FileName;
            }
        }
    }
}
