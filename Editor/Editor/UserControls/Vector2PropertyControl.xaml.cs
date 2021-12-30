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
    /// Interaction logic for Vector2PropertyControl.xaml
    /// </summary>
    public partial class Vector2PropertyControl : UserControl
    {
        public Vector2PropertyControl()
        {
            InitializeComponent();
        }

        public Vector2PropertyControlViewModel Vector2ControlViewModel
        {
            get => (Vector2PropertyControlViewModel)GetValue(Vector2ControlViewModelProperty);
            set => SetValue(Vector2ControlViewModelProperty, value);
        }

        public static readonly DependencyProperty Vector2ControlViewModelProperty = DependencyProperty.Register(nameof(Vector2ControlViewModel),
            typeof(Vector2PropertyControlViewModel), typeof(Vector2PropertyControl), new PropertyMetadata(OnViewModelChanged));
        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Vector2PropertyControl view || e.NewValue is not Vector2PropertyControlViewModel viewModel)
            {
                return;
            }

            view.DataContext = viewModel;
        }
    }
}
