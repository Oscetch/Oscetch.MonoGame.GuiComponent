using Editor.Modals;
using Editor.MonoGameControls;
using Editor.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
            EditorSettings.Load().ProjectChanged += EditorView_ProjectChanged;
            Loaded += EditorView_Loaded;
        }

        private void EditorView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            var setup = new ModalSetupWindow();
            if (setup.ShowDialog() != true)
            {
                Environment.Exit(0);
            }
        }

        private void EditorView_ProjectChanged(object sender, EventArgs e)
        {
            foreach (var existing in RootDock.Children.OfType<MonoGameContentControl>())
            {
                RootDock.Children.Remove(existing);
            }
            RootDock.Children.Add(new MonoGameContentControl());
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
