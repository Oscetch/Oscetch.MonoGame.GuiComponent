using Oscetch.MonoGame.GuiComponent.Interfaces;

namespace TetrisClone
{
    public class GameToGuiService : IGameToGuiService
    {
        public TetrisEngine Engine { get; }

        internal GameToGuiService(TetrisEngine engine)
        {
            Engine = engine;
        }
    }
}
