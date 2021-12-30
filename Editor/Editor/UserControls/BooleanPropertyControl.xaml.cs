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
    /// Interaction logic for BooleanPropertyControl.xaml
    /// </summary>
    public partial class BooleanPropertyControl : UserControl
    {
        public BooleanPropertyControl()
        {
            InitializeComponent();
        }

        public BooleanPropertyControlViewModel BooleanControlViewModel
        {
            get => (BooleanPropertyControlViewModel)GetValue(BooleanControlViewModelProperty);
            set => SetValue(BooleanControlViewModelProperty, value);
        }

        public static readonly DependencyProperty BooleanControlViewModelProperty = DependencyProperty.Register(nameof(BooleanControlViewModel),
            typeof(BooleanPropertyControlViewModel), typeof(BooleanPropertyControl), new PropertyMetadata(OnViewModelChanged));
        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not BooleanPropertyControl view || e.NewValue is not BooleanPropertyControlViewModel viewModel)
            {
                return;
            }

            view.DataContext = viewModel;
        }
    }
}
