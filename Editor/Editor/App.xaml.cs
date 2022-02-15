using Editor.Services;
using Editor.ViewModels;
using System;
using System.Windows;

namespace Editor
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //var test = new DIService();
            //var resolveTest = test.Resolve<LeftPanelViewModel>();

            base.OnStartup(e);
        }
    }
}
