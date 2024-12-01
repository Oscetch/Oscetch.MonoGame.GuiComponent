using Oscetch.MonoGame.GuiComponent.Interfaces;
using Oscetch.MonoGame.GuiComponent;
using Oscetch.MonoGame.GuiComponent.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Editor.Handlers;
using System;

namespace Editor.Models
{
    public class ControlTreeItem
    {
        private readonly GuiControl<IGameToGuiService> _model;

        public string Name { get; }
        public ICommand OnClickCommand { get; }
        public List<ControlTreeItem> Children { get; }

        public ControlTreeItem(GuiControl<IGameToGuiService> model, Action<ulong> onClick)
        {
            _model = model;
            Name = string.IsNullOrEmpty(_model.Name) ? model.Id.ToString() : model.Name;
            Children = model.Children.Select(c => new ControlTreeItem(c, onClick)).ToList();
            OnClickCommand = new CommandHandler(() => onClick(_model.Id));
        }
    }
}
