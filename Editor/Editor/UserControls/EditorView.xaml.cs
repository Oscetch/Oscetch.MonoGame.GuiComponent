using Editor.Modals;
using Editor.MonoGameControls;
using System;
using System.ComponentModel;
using System.Linq;
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
            foreach (var existing in RootDock.Children.OfType<MonoGameContentControl>().ToList())
            {
                existing.Dispose();
                RootDock.Children.Remove(existing);
            }
            RootDock.Children.Add(new MonoGameContentControl());
        }
    }
}
