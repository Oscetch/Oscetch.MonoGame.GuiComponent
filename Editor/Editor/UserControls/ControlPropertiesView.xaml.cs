using Editor.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for ControlPropertiesControl.xaml
    /// </summary>
    public partial class ControlPropertiesView : UserControl
    {
        public ControlPropertiesView()
        {
            InitializeComponent();
        }

        public ControlPropertiesViewModel ControlPropertiesViewModel
        {
            get => (ControlPropertiesViewModel)GetValue(ControlPropertiesViewModelProperty);
            set => SetValue(ControlPropertiesViewModelProperty, value);
        }

        public static readonly DependencyProperty ControlPropertiesViewModelProperty = DependencyProperty.Register(nameof(ControlPropertiesViewModel),
            typeof(ControlPropertiesViewModel), typeof(ControlPropertiesView), new PropertyMetadata(OnViewModelChanged));
        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ControlPropertiesView view || e.NewValue is not ControlPropertiesViewModel viewModel)
            {
                return;
            }

            view.DataContext = viewModel;
        }
    }
}
