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
    /// Interaction logic for TopLeftPanelView.xaml
    /// </summary>
    public partial class TopLeftPanelView : UserControl
    {
        public TopLeftPanelView()
        {
            InitializeComponent();
        }

        public TopLeftViewModel TopLeftViewModel
        {
            get => (TopLeftViewModel)GetValue(TopLeftViewModelProperty);
            set => SetValue(TopLeftViewModelProperty, value);
        }

        public static readonly DependencyProperty TopLeftViewModelProperty = DependencyProperty.Register(nameof(TopLeftViewModel),
            typeof(TopLeftViewModel), typeof(TopLeftPanelView), new PropertyMetadata(OnViewModelChanged));
        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TopLeftPanelView view || e.NewValue is not TopLeftViewModel viewModel)
            {
                return;
            }

            view.DataContext = viewModel;
        }
    }
}
