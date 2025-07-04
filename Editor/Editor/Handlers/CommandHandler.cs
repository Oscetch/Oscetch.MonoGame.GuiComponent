using System;
using System.Windows.Input;

namespace Editor.Handlers
{
    public class CommandHandler : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool> _canExecute;

        public CommandHandler(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public CommandHandler(Action action)
        {
            _action = action;
            _canExecute = () => true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual bool CanExecute(object parameter)
        {
            return _canExecute.Invoke();
        }

        public virtual void Execute(object parameter)
        {
            _action();
        }
    }

    public class CommandHandler<T> : ICommand
    {
        private readonly Action<T> _action;
        private readonly Func<T, bool> _canExecute;

        public CommandHandler(Action<T> action, Func<T, bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public CommandHandler(Action<T> action)
        {
            _action = action;
            _canExecute = _ => true;
        }

        public bool CanExecute(object parameter)
        {
            return parameter is T typed && _canExecute(typed);
        }

        public void Execute(object parameter)
        {
            if (parameter is T typed) _action(typed);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
