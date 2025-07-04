using Editor.Handlers;
using Editor.Modals;
using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Oscetch.MonoGame.GuiComponent.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Editor.ViewModels
{
    public class TopLeftViewModel : ViewModel
    {
        private readonly EditorViewModel _editorViewModel;

        public ICommand SelectParentCommand { get; }
        public ICommand CreateNewControlCommand { get; }
        public ICommand ImportCanvasCommand { get; }
        public ICommand MoveSelectedCommand { get; }

        public TopLeftViewModel(EditorViewModel editorViewModel)
        {
            _editorViewModel = editorViewModel;
            SelectParentCommand = new CommandHandler(_editorViewModel.SelectParent, () => _editorViewModel.IsInitialized
                && _editorViewModel.SelectedParameters != null);
            CreateNewControlCommand = new CommandHandler(CreateChildControl, () => _editorViewModel.IsInitialized);
            ImportCanvasCommand = new CommandHandler(ImportCanvas, () => _editorViewModel.IsInitialized);

            MoveSelectedCommand = new CommandHandler(MoveSelected, () => _editorViewModel.IsInitialized
                && _editorViewModel.SelectedParameters != null);
        }

        private void CreateChildControl()
        {
            var newParameters = new GuiControlParameters(_editorViewModel.Parameters.OriginalResolution)
            {
                Size = new Vector2(100),
                Position = _editorViewModel.CenterOfScreen - new Vector2(50),
                TextPosition = _editorViewModel.CenterOfScreen,
                HasBorder = true,
                BorderColor = Color.White,
                SpriteFont = ProjectSettings.GetSettings().FontPath
            };

            if (_editorViewModel.SelectedParameters == null)
            {
                _editorViewModel.Parameters.ChildControls.Add(newParameters);
            }
            else
            {
                _editorViewModel.SelectedParameters.ChildControls.Add(newParameters);
            }

            _editorViewModel.ResetWithParameters(_editorViewModel.Parameters);
        }

        private void ImportCanvas() 
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
                if (_editorViewModel.SelectedParameters == null)
                {
                    _editorViewModel.Parameters.ChildControls.AddRange(parameters);
                }
                else
                {
                    _editorViewModel.SelectedParameters.ChildControls.AddRange(parameters);
                }

                _editorViewModel.ResetWithParameters(_editorViewModel.Parameters);
            }
            catch (Exception e)
            {
                Debug.Write(e);
                MessageBox.Show("Could not load file");
            }
        }
    
        private void MoveSelected()
        {
            var selected = _editorViewModel.SelectedParameters;
            var all = _editorViewModel.GetAllExceptSelected();
            var dialog = new MoveDialog(all);
            if (dialog.ShowDialog() != true) return;
            var newlySelected = dialog.Selected.Control;
            _editorViewModel.RemoveSelected();
            newlySelected.Parameters.ChildControls.Add(selected);
            _editorViewModel.ResetWithParameters(_editorViewModel.Parameters);
        }

        private static IEnumerable<GuiControlParameters> AllParametersExcept(GuiControlParameters current, GuiControlParameters except)
        {
            if (current.Name == except.Name) yield break;

            yield return current;
            foreach (var child in current.ChildControls)
            {
                foreach (var c in AllParametersExcept(child, except))
                {
                    yield return c;
                }
            }
        }
    }
}
