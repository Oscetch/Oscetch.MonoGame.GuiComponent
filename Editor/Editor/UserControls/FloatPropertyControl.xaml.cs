using Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for FloatPropertyControl.xaml
    /// </summary>
    public partial class FloatPropertyControl : UserControl
    {
        public FloatPropertyControl()
        {
            InitializeComponent();
        }

        public FloatPropertyControlViewModel FloatControlViewModel
        {
            get => (FloatPropertyControlViewModel)GetValue(FloatControlViewModelProperty);
            set => SetValue(FloatControlViewModelProperty, value);
        }

        public static readonly DependencyProperty FloatControlViewModelProperty = DependencyProperty.Register(nameof(FloatControlViewModel),
            typeof(FloatPropertyControlViewModel), typeof(FloatPropertyControl), new PropertyMetadata(OnViewModelChanged));
        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FloatPropertyControl view || e.NewValue is not FloatPropertyControlViewModel viewModel)
            {
                return;
            }

            view.DataContext = viewModel;
        }
    }
}
