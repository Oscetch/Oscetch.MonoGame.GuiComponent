using Editor.Handlers;
using Editor.Modals;
using Microsoft.Win32;
using Newtonsoft.Json;
using Oscetch.MonoGame.GuiComponent.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Editor.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public EditorViewModel EditorViewModel { get; }
        public LeftPanelViewModel LeftPanelViewModel { get; }

        public ICommand CreateScriptCommand { get; }
        public ICommand EditScriptCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand TestCommand { get; }

        public MainWindowViewModel()
        {
            EditorViewModel = new EditorViewModel();
            LeftPanelViewModel = new LeftPanelViewModel(EditorViewModel);

            CreateScriptCommand = new CommandHandler(OpenScriptControl);
            SaveCommand = new CommandHandler(OnSave);
            LoadCommand = new CommandHandler(OnLoad);
            TestCommand = new CommandHandler(OnTest);
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
                    IsEnabled = true
                };
                EditorViewModel.ResetWithParameters(newBase);
            }
            catch
            {
                MessageBox.Show("Could not load file");
            }
        }

        private void OnTest()
        {
            var serialized = JsonConvert.SerializeObject(EditorViewModel.Parameters.ChildControls);
            File.WriteAllText("testcanvas.json", serialized);

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = $"{typeof(TetrisClone.App).Namespace}.exe"
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

            var serialized = JsonConvert.SerializeObject(EditorViewModel.Parameters.ChildControls);
            File.WriteAllText(saveFileDialog.FileName, serialized);
        }

        private void OpenScriptControl()
        {
            var scriptWindow = new ModalScriptWindow("ScriptTemplates/ScriptTemplate.txt");
            scriptWindow.ShowDialog();
        }
    }
}
