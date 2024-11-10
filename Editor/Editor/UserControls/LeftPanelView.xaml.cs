using Editor.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
