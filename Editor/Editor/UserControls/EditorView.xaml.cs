using Editor.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for EditorView.xaml
    /// </summary>
    public partial class EditorView : UserControl
    {
        public EditorView()
        {
            InitializeComponent();
        }

        public EditorViewModel EditorViewModel 
        { 
            get => (EditorViewModel)GetValue(EditorViewModelProperty); 
            set => SetValue(EditorViewModelProperty, value); 
        }

        public static readonly DependencyProperty EditorViewModelProperty = DependencyProperty.Register(nameof(EditorViewModel),
            typeof(EditorViewModel), typeof(EditorView), new PropertyMetadata(OnViewModelChanged));
        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not EditorView view || e.NewValue is not EditorViewModel viewModel)
            {
                return;
            }

            view.DataContext = viewModel;
        }
    }
}
