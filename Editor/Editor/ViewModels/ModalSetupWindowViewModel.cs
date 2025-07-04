using Editor.Extensions;
using Editor.Handlers;
using Editor.Modals;
using Editor.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Editor.ViewModels
{
    public class ModalSetupWindowViewModel : ViewModel
    {
        private bool _useCustomContentManager;
        private string _contentManagerPath;

        public ICommand OpenExisting { get; }
        public ICommand FindProject { get; }
        public ICommand BrowseForCustomContentManager { get; }

        public bool UseCustomContentManager 
        { 
            get => _useCustomContentManager;
            set
            {
                _useCustomContentManager = value;
                OnPropertyChanged();
            }
        }
        public string ContentManagerPath 
        { 
            get => _contentManagerPath;
            set
            {
                _contentManagerPath = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<SetupProjectItem> KnownProjects { get; }

        public ModalSetupWindowViewModel()
        {
            FindProject = new CommandHandler<Window>(FindProjectFromPath);
            OpenExisting = new CommandHandler<Window>(OpenExistingFromPath);
            BrowseForCustomContentManager = new CommandHandler(BrowseForContentManager);
            var knownProjects = EditorSettings.Load().KnownProjectPaths.Select(x => new SetupProjectItem(x, window => OpenFromPath(x, window)));
            KnownProjects = [..knownProjects];
        }

        private void OpenExistingFromPath(Window window)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select existing project",
                Filter = "Oscetch settings(*.oscetchjson)|*.oscetchjson",
                Multiselect = false,
            };
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }
            OpenFromPath(openFileDialog.FileName, window);
        }

        private static void OpenFromPath(string path, Window window)
        {
            ProjectSettings.GetSettings(path);
            EditorSettings.Load().LoadProject(path);
            window.DialogResult = true;
        }

        private void BrowseForContentManager()
        {
            var openFileDialog = new OpenFileDialog 
            {
                Title = "Select content manager",
                Filter = "Content manager(MGCB.exe)|*.exe",
                Multiselect = false,
            };
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }
            ContentManagerPath = openFileDialog.FileName;
        }

        private void FindProjectFromPath(Window window)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select game sln",
                Filter = "C# solution(*.sln)|*.sln",
                Multiselect = false,
            };
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            var projectDir = Path.GetDirectoryName(openFileDialog.FileName);

            var suggestedProjectPath = Path.Join(projectDir, "OscetchUi");
            
            var contentPath = FindDirInPath(projectDir, "Content", ["bin", "obj"]);
            var csProjPath = Path.GetDirectoryName(contentPath);
            var projBinPath = FindDirInPath(csProjPath, "bin");
            
            var contentInBinPath = FindDirInPath(projBinPath, "Content");

            var mgcbPath = GetMgcbPath();

            var font = contentPath.FindFileInPath(".spritefont", ["bin", "obj"]);
            var baseDll = csProjPath.FindFileInPath($"{new DirectoryInfo(csProjPath).Name}.dll");

            var viewModel = new EditProjectSettingsDialogViewModel(suggestedProjectPath, contentPath, contentInBinPath, mgcbPath, font, baseDll);
            var dialog = new EditProjectSettingsDialog("New project", viewModel);
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            window.DialogResult = true;
        }

        private static string FindDirInPath(string path, string directoryName, List<string> except = null)
        {
            foreach (var dir in Directory.GetDirectories(path))
            {
                if (dir.EndsWith(directoryName)) return dir;
                if (except?.Any(dir.EndsWith) == true) continue;
                var childMatch = FindDirInPath(dir, directoryName, except);
                if (childMatch != null) return childMatch;
            }
            return null;
        }

        private string GetMgcbPath()
        {
            if (UseCustomContentManager)
            {
                return ContentManagerPath;
            }
            else
            {
                return Environment.OSVersion.Platform == PlatformID.Win32NT
                    ? Path.GetFullPath("Tools/mgcb.exe")
                    : Path.GetFullPath("Tools/mgcb");
            }
        }
    }
}
