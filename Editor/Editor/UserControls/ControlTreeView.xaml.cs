using Editor.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for ControlTreeView.xaml
    /// </summary>
    public partial class ControlTreeView : UserControl
    {
        public ControlTreeView()
        {
            InitializeComponent();
        }

        public ControlTreeViewModel ControlTreeViewModel 
        {
            get => (ControlTreeViewModel)GetValue(ControlTreeViewModelProperty);
            set => SetValue(ControlTreeViewModelProperty, value);
        }

        public static readonly DependencyProperty ControlTreeViewModelProperty = DependencyProperty.Register(nameof(ControlTreeViewModel), 
            typeof(ControlTreeViewModel), typeof(ControlTreeView), new PropertyMetadata(OnViewModelChanged));

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ControlTreeView view || e.NewValue is not ControlTreeViewModel viewModel) 
            {
                return;
            } 
            view.DataContext = viewModel;
        }
    }
}
