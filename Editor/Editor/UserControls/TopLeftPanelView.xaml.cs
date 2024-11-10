using Editor.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
