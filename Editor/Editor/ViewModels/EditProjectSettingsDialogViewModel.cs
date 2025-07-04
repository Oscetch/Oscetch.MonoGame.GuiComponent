using Editor.Extensions;
using Editor.Handlers;
using Editor.MgcbStuff;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Editor.ViewModels
{
    public class EditProjectSettingsDialogViewModel : ViewModel
    {
        private string _projectPath;
        private string _contentPath;
        private string _contentBinPath;
        private string _mgcbPath;
        private string _defaultFont;
        private string _scriptsAssemblyName;
        private string _scriptOutputDllPath;
        private string _scriptsDirectory;
        private string _baseDllPath;

        private bool _addDefaultFont;

        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                _projectPath = value;
                OnPropertyChanged();
            }
        }
        public string ContentPath
        {
            get => _contentPath;
            set
            {
                _contentPath = value;
                OnPropertyChanged();
            }
        }
        public string ContentBinPath
        {
            get => _contentBinPath;
            set
            {
                _contentBinPath = value;
                OnPropertyChanged();
            }
        }
        public string MgcbPath
        {
            get => _mgcbPath;
            set
            {
                _mgcbPath = value;
                OnPropertyChanged();
            }
        }
        public string DefaultFont
        {
            get => _defaultFont;
            set
            {
                _defaultFont = value;
                OnPropertyChanged();
            }
        }
        public string ScriptsAssemblyName
        {
            get => _scriptsAssemblyName;
            set
            {
                _scriptsAssemblyName = value;
                OnPropertyChanged();
            }
        }
        public string ScriptOutputDllPath
        {
            get => _scriptOutputDllPath;
            set
            {
                _scriptOutputDllPath = value;
                OnPropertyChanged();
            }
        }
        public string ScriptsDirectory
        {
            get => _scriptsDirectory;
            set
            {
                _scriptsDirectory = value;
                OnPropertyChanged();
            }
        }
        public string BaseDllPath
        {
            get => _baseDllPath;
            set
            {
                _baseDllPath = value;
                OnPropertyChanged();
            }
        }

        public bool AddDefaultFont
        {
            get => _addDefaultFont;
            set
            {
                _addDefaultFont = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DontAddDefaultFont));
            }
        }

        public bool DontAddDefaultFont => !_addDefaultFont;

        public ICommand SelectProjectPath { get; }
        public ICommand SelectContentPath { get; }
        public ICommand SelectContentBinPath { get; }
        public ICommand SelectMgcbPath { get; }
        public ICommand SelectDefaultFont { get; }
        public ICommand SetOutputDllPath { get; }
        public ICommand SelectScriptsDirectory { get; }
        public ICommand SelectDllPath { get; }
        public ICommand OnOk { get; }

        public EditProjectSettingsDialogViewModel(string projectPath, string contentPath, string contentBinPath, string mgcbPath, string defaultFont, string baseDllPath)
        {
            ProjectPath = projectPath;
            ContentPath = contentPath;
            ContentBinPath = contentBinPath;
            MgcbPath = mgcbPath;
            DefaultFont = defaultFont;
            AddDefaultFont = string.IsNullOrEmpty(DefaultFont);
            ScriptsAssemblyName = "Default";
            ScriptOutputDllPath = Path.Join(projectPath, "Oscetch.Canvas.dll");
            ScriptsDirectory = Path.Join(projectPath, "Scripts");
            BaseDllPath = baseDllPath;

            SelectProjectPath = new CommandHandler(() => SelectDirectory("Select project path", p => ProjectPath = p));
            SelectContentPath = new CommandHandler(() => SelectDirectory("Select content path", p => ContentPath = p));
            SelectContentBinPath = new CommandHandler(() => SelectDirectory("Select content bin path", p => ContentBinPath = p));
            SelectMgcbPath = new CommandHandler(() => SelectFilePath("Select content manager", "Content manager(MGCB.exe)|*.exe", p => MgcbPath = p));
            SelectDefaultFont = new CommandHandler(() => SelectFilePath("Select default font", "Spritefont(*.spritefont)|*.spritefont", p => DefaultFont = p));
            SetOutputDllPath = new CommandHandler(() => SetFilePath("Set scripts output dll path", "Dynamic link library(*.dll)|*.dll", p => ScriptOutputDllPath = p));
            SelectScriptsDirectory = new CommandHandler(() => SelectDirectory("Select scripts directory", p => ScriptsDirectory = p));
            SelectDllPath = new CommandHandler(() => SelectFilePath("Game dll path", "Dynamic link library(*.dll)|*.dll", p => BaseDllPath = p));

            OnOk = new CommandHandler<Window>(OnOKClicked);
        }

        private void OnOKClicked(Window window)
        {
            if (!IsInputValid())
            {
                MessageBox.Show("Empty input");
                return;
            }
            if (!ValidatePaths())
            {
                MessageBox.Show("Either MGCB, Content directory, Content.mgcb, or the Game dll path did not exist");
                return;
            }
            CreateMissingPaths();

            if (AddDefaultFont)
            {
                AddDefaultFontToContent();
            }

            var settings = ProjectSettings.GetSettings(Path.Join(ProjectPath, "settings.oscetchjson"));
            settings.AssemblyName = ScriptsAssemblyName;
            settings.OutputPath = ScriptOutputDllPath;
            settings.ScriptsDir = ScriptsDirectory;
            settings.ContentBinPath = ContentBinPath;

            var relativeToContent = Path.GetRelativePath(ContentPath, DefaultFont);
            var parent = Path.GetDirectoryName(relativeToContent);
            settings.FontPath = Path.Join(parent, Path.GetFileNameWithoutExtension(relativeToContent));

            settings.MgcbPath = MgcbPath;
            settings.ContentPath = ContentPath;
            settings.GameDllPath = BaseDllPath;

            ProjectSettings.Save();
            EditorSettings.Load().LoadProject(settings.SettingsPath);

            window.DialogResult = true;
        }

        private void AddDefaultFontToContent()
        {
            var defaultFontPath = Path.GetFullPath("ContentDefaults/DefaultFont.spritefont");
            Mgcb.AddSpriteFont(defaultFontPath, _contentPath, MgcbPath);
            DefaultFont = defaultFontPath;
        }

        private void CreateMissingPaths()
        {
            List<string> directories = [_projectPath, _scriptsDirectory];

            foreach (var directory in directories) 
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }

        private bool ValidatePaths()
        {
            List<string> filesThatShouldExist = [_mgcbPath, _baseDllPath, _contentPath.FindFileInPath(".mgcb")];
            if (!filesThatShouldExist.All(File.Exists)) return false;
            if (!Directory.Exists(ContentPath)) return false;

            return true;
        }

        private bool IsInputValid()
        {
            List<string> strings = 
                [
                    _projectPath,
                    _contentPath,
                    _contentBinPath,
                    _mgcbPath,
                    _scriptsAssemblyName,
                    _scriptOutputDllPath,
                    _scriptsDirectory,
                    _baseDllPath
                ];
            if (DontAddDefaultFont)
            {
                strings.Add(_defaultFont);
            }
            return !strings.Any(string.IsNullOrWhiteSpace);
        }

        private static void SelectDirectory(string title, Action<string> onSelected)
        {
            var openDirectoryDialog = new OpenFolderDialog
            {
                Title = title,
                Multiselect = false,
            };
            if (openDirectoryDialog.ShowDialog() != true) return;
            onSelected?.Invoke(openDirectoryDialog.FolderName);
        }

        private static void SelectFilePath(string title, string filter, Action<string> onSelected)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
                Multiselect = false,
            };
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }
            onSelected?.Invoke(openFileDialog.FileName);
        }

        private static void SetFilePath(string title, string filter, Action<string> onSelected)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = title,
                Filter = filter,
            };
            if (saveFileDialog.ShowDialog() != true)
            {
                return;
            }
            onSelected?.Invoke(saveFileDialog.FileName);
        }
    }
}
