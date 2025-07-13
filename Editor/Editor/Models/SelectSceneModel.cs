using Editor.Handlers;
using Oscetch.ScriptComponent;
using System;
using System.Windows;
using System.Windows.Input;

namespace Editor.Models
{
    public class SelectSceneModel
    {
        public ScriptReference ScriptReference { get; }
        public string Name { get; }
        public ICommand Command { get; }

        public SelectSceneModel(ScriptReference scriptReference, Action<Window> onClick)
        {
            ScriptReference = scriptReference;
            Name = scriptReference.ToString();
            Command = new CommandHandler<Window>(onClick);
        }
    }
}
