using Editor.Handlers;
using Editor.Modals;
using Microsoft.Win32;
using Newtonsoft.Json;
using Oscetch.MonoGame.GuiComponent.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Editor.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private readonly EditorViewModel _editorViewModel;

        public ICommand Setup { get; }
        public ICommand ChangeSettings { get; }
        public ICommand CreateScriptCommand { get; }
        public ICommand EditScriptCommand { get; }
        public ICommand ChangeBaseScriptCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand TestCommand { get; }
        public ICommand EditTestExePathCommand { get; }
        public ICommand EditTestJsonPathCommand { get; }
        public ICommand EditDllOutputPath { get; }

        public MainWindowViewModel(EditorViewModel editorViewModel)
        {
            _editorViewModel = editorViewModel;

            CreateScriptCommand = new CommandHandler(OpenScriptControl);
            EditScriptCommand = new CommandHandler(OnEditScript);
            SaveCommand = new CommandHandler(OnSave);
            LoadCommand = new CommandHandler(OnLoad);
            TestCommand = new CommandHandler(OnTest);
            EditTestExePathCommand = new CommandHandler(OnChangeExeTestPath);
            EditTestJsonPathCommand = new CommandHandler(OnChangeJsonTestPath);
            EditDllOutputPath = new CommandHandler(OnChangeDllPath);
            Setup = new CommandHandler(OpenSetup);
            ChangeSettings = new CommandHandler(ChangeProjectSettings);
        }

        private void ChangeProjectSettings()
        {
            var projectSettings = ProjectSettings.GetSettings();
            var editSettingsViewModel = new EditProjectSettingsDialogViewModel(
                Path.GetDirectoryName(projectSettings.SettingsPath),
                projectSettings.ContentPath,
                projectSettings.ContentBinPath,
                projectSettings.MgcbPath,
                Path.ChangeExtension(Path.Join(projectSettings.ContentPath, projectSettings.FontPath), "spritefont"),
                projectSettings.GameDllPath,
                projectSettings.Resolution.X,
                projectSettings.Resolution.Y
            );
            var editSettings = new EditProjectSettingsDialog("Edit settings", editSettingsViewModel);
            editSettings.ShowDialog();
        }

        private void OpenSetup()
        {
            var setupWindow = new ModalSetupWindow();
            setupWindow.ShowDialog();
        }

        private void OnLoad()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "JSON file(*.json)|*.json"
            };
            if (!(openFileDialog.ShowDialog() ?? false))
            {
                return;
            }

            try
            {
                var json = File.ReadAllText(openFileDialog.FileName);
                var parameters = JsonConvert.DeserializeObject<List<GuiControlParameters>>(json);
                var newBase = new GuiControlParameters(new Microsoft.Xna.Framework.Vector2(1280, 720))
                {
                    ChildControls = parameters,
                    IsVisible = true,
                    IsEnabled = true,
                    SpriteFont = ProjectSettings.GetSettings().FontPath
                };
                _editorViewModel.ResetWithParameters(newBase);
            }
            catch (Exception)
            {
                MessageBox.Show("Could not load file");
            }
        }

        private void OnTest()
        {
            var settings = ProjectSettings.GetSettings();
            if (settings.TestJsonPath == null)
            {
                MessageBox.Show("You must select the json output path");
                return;
            }
            if (settings.TestExePath == null)
            {
                MessageBox.Show("You must select the test exe output path");
                return;
            }

            var serialized = JsonConvert.SerializeObject(_editorViewModel.Parameters.ChildControls);
            File.WriteAllText(settings.TestJsonPath, serialized);

            var binTargetPath = Path.Join(Path.GetDirectoryName(settings.TestExePath), Path.GetFileName(settings.OutputPath));
            File.Copy(settings.OutputPath, binTargetPath, true);

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = settings.TestExePath,
                        WorkingDirectory = Path.GetDirectoryName(settings.TestExePath),
                    }
                };

                process.Start();
                process.WaitForExit();
            } 
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void OnSave()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON file(*.json)|*.json"
            };
            if (!(saveFileDialog.ShowDialog() ?? false))
            {
                return;
            }

            var serialized = JsonConvert.SerializeObject(_editorViewModel.Parameters.ChildControls);
            File.WriteAllText(saveFileDialog.FileName, serialized);
        }

        private void OpenScriptControl()
        {
            var scriptWindow = new ModalScriptWindow("ScriptTemplates/ScriptTemplate.txt");
            scriptWindow.ShowDialog();
        }

        private void OnEditScript()
        {
            var settings = ProjectSettings.GetSettings();
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Path.GetFullPath(settings.ScriptsDir),
                Filter = "C# file(*.cs)|*.cs",
                Multiselect = false,
            };
            if (!(openFileDialog.ShowDialog() ?? false))
            {
                return;
            }
            var scriptWindow = new ModalScriptWindow(openFileDialog.FileName);
            scriptWindow.ShowDialog();
        }

        private void OnChangeExeTestPath()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Game launcher (*.exe)|*.exe",
                Multiselect = false
            };
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }
            ProjectSettings.GetSettings().TestExePath = openFileDialog.FileName;
            ProjectSettings.Save();
        }

        private void OnChangeJsonTestPath()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Canvas file (*.json)|*.json",
            };
            if (saveFileDialog.ShowDialog() != true) { return; }
            ProjectSettings.GetSettings().TestJsonPath = saveFileDialog.FileName;
            ProjectSettings.Save();
        }

        private void OnChangeDllPath()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Library (*.dll)|*.dll"
            };
            if (saveFileDialog.ShowDialog() != true) return;
            ProjectSettings.GetSettings().OutputPath = saveFileDialog.FileName;
            ProjectSettings.Save();
        }
    }
}
