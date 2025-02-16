using Oscetch.MonoGame.GuiComponent;
using Oscetch.MonoGame.GuiComponent.Interfaces;

namespace Editor.Models
{
    public class SelectableControlModel
    {
        public bool IsSelected { get; set; }
        public GuiControl<IGameToGuiService> Control{ get; }
        public string Name => Control.Name;

        public SelectableControlModel(GuiControl<IGameToGuiService> control)
        {
            Control = control;
        }
    }
}
