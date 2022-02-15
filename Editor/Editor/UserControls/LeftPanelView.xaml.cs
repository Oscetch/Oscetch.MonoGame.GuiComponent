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
    /// Interaction logic for LeftPanelView.xaml
    /// </summary>
    public partial class LeftPanelView : UserControl
    {
        public LeftPanelView()
        {
            InitializeComponent();
        }

        public LeftPanelViewModel LeftPanelViewModel
        {
            get => (LeftPanelViewModel)GetValue(LeftPanelViewModelProperty);
            set => SetValue(LeftPanelViewModelProperty, value);
        }

        public static readonly DependencyProperty LeftPanelViewModelProperty = DependencyProperty.Register(nameof(LeftPanelViewModel),
            typeof(LeftPanelViewModel), typeof(LeftPanelView), new PropertyMetadata(OnViewModelChanged));
        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not LeftPanelView view || e.NewValue is not LeftPanelViewModel viewModel)
            {
                return;
            }

            view.DataContext = viewModel;
        }
    }
}
