using Editor.ViewModels;
using Oscetch.MonoGame.GuiComponent.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Editor.Modals
{
    /// <summary>
    /// Interaction logic for TestLayoutWindow.xaml
    /// </summary>
    public partial class TestLayoutWindow : Window
    {
        public TestLayoutWindow(dynamic scene, List<GuiControlParameters> parameters)
        {
            var settings = ProjectSettings.GetSettings();
            InitializeComponent();
            RootDock.Height = settings.Resolution.Y;
            RootDock.Width = settings.Resolution.X;
            DataContext = new TestLayoutWindowViewModel(scene, parameters);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            MonoGameContent.Dispose();
        }
    }
}
