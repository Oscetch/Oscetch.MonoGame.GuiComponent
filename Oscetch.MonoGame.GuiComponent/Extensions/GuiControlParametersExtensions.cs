using Oscetch.MonoGame.GuiComponent.Interfaces;
using Oscetch.MonoGame.GuiComponent.Models;
using System.Collections.Generic;

namespace Oscetch.MonoGame.GuiComponent.Extensions
{
    public static class GuiControlParametersExtensions
    {
        public static GuiCanvas<T> ToCanvas<T>(this IEnumerable<GuiControlParameters> parameters, T gameToGuiService) where T : IGameToGuiService =>
            new (() => [.. parameters], gameToGuiService);
    }
}
