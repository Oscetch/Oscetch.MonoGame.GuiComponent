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
    /// Interaction logic for TextPropertyControl.xaml
    /// </summary>
    public partial class TextPropertyControl : UserControl
    {
        public TextPropertyControl()
        {
            InitializeComponent();
        }

        public TextPropertyControlViewModel TextControlViewModel
        {
            get => (TextPropertyControlViewModel)GetValue(Vector2ControlViewModelProperty);
            set => SetValue(Vector2ControlViewModelProperty, value);
        }

        public static readonly DependencyProperty Vector2ControlViewModelProperty = DependencyProperty.Register(nameof(TextControlViewModel),
            typeof(TextPropertyControlViewModel), typeof(TextPropertyControl), new PropertyMetadata(OnViewModelChanged));
        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextPropertyControl view || e.NewValue is not TextPropertyControlViewModel viewModel)
            {
                return;
            }

            view.DataContext = viewModel;
        }
    }
}
