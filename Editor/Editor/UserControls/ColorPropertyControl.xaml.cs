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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for ColorPropertyControl.xaml
    /// </summary>
    public partial class ColorPropertyControl : UserControl
    {
        public ColorPropertyControl()
        {
            InitializeComponent();
        }

        public ColorPropertyControlViewModel ColorControlViewModel
        {
            get => (ColorPropertyControlViewModel)GetValue(ColorControlViewModelProperty);
            set => SetValue(ColorControlViewModelProperty, value);
        }

        public static readonly DependencyProperty ColorControlViewModelProperty = DependencyProperty.Register(nameof(ColorControlViewModel),
            typeof(ColorPropertyControlViewModel), typeof(ColorPropertyControl), new PropertyMetadata(OnViewModelChanged));
        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ColorPropertyControl view || e.NewValue is not ColorPropertyControlViewModel viewModel)
            {
                return;
            }

            view.DataContext = viewModel;
        }
    }
}
