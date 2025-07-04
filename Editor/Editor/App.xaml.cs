﻿using Editor.Services;
using Editor.ViewModels;
using System.Windows;

namespace Editor
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NinjectKernel.Init(new ViewModelConfiguration());

            base.OnStartup(e);
        }
    }
}
