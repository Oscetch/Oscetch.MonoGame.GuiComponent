using Editor.Handlers;
using System;
using System.Windows;
using System.Windows.Input;

namespace Editor.Models
{
    public class SetupProjectItem(string name, Action<Window> onClick)
    {
        public string Name { get; } = name;
        public ICommand OnClick { get; } = new CommandHandler<Window>(onClick);
    }
}
