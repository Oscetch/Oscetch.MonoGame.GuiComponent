using Editor.Handlers;
using Microsoft.Xna.Framework;
using Oscetch.MonoGame.GuiComponent.Models;
using System.Windows.Input;

namespace Editor.ViewModels
{
    public class TopLeftViewModel : ViewModel
    {
        private readonly EditorViewModel _editorViewModel;

        public ICommand SelectParentCommand { get; }
        public ICommand CreateNewControlCommand { get; }

        public TopLeftViewModel(EditorViewModel editorViewModel)
        {
            _editorViewModel = editorViewModel;
            SelectParentCommand = new CommandHandler(_editorViewModel.SelectParent, () => _editorViewModel.IsInitialized
                && _editorViewModel.SelectedParameters != null);
            CreateNewControlCommand = new CommandHandler(CreateChildControl, () => _editorViewModel.IsInitialized);
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
                SpriteFont = Settings.GetSettings().FontPath
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
    }
}
