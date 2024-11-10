using Editor.Modals;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;

namespace Editor
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var settings = Settings.GetSettings();
            if (settings.ContentPath == null)
            {
                var directoryDialog = new OpenFolderDialog
                {
                    Title = "Select Content Path",
                    Multiselect = false,
                };
                if (directoryDialog.ShowDialog() != true)
                {
                    Environment.Exit(0);
                    return;
                }
                settings.ContentPath = directoryDialog.FolderName;
                Settings.Save();
            }
            if (settings.BaseScriptReference == null)
            {
                var openFileDialog = new OpenFileDialog
                {
                    Title = "Select game exe or dll",
                    Filter = "Reference script(*.dll,*.exe)|*.dll;*.exe",
                    Multiselect = false,
                };
                if (openFileDialog.ShowDialog() != true)
                {
                    Environment.Exit(0);
                    return;
                }
                var scriptReferenceWindow = new ReferenceScriptTypeDialog(openFileDialog.FileName);
                scriptReferenceWindow.ShowDialog();

                settings.BaseScriptReference = scriptReferenceWindow.ScriptReferenceCheckedModels
                    ?.FirstOrDefault(x => x.IsSelected)
                    ?.ScriptReference;
                Settings.Save();

                if (settings.BaseScriptReference == null)
                {
                    Environment.Exit(0);
                    return;
                }
            }

            base.OnStartup(e);
        }
    }
}
