using System;
using System.Windows.Input;

namespace TeamGenerator.Shell
{
    public class Command : ICommand
    {
        private readonly Action<object> method;
        private readonly Func<object, bool> canExecuteMethod;

        public Command(Action<object> method, Func<object, bool> canExecuteMethod)
        {
            this.method = method;
            this.canExecuteMethod = canExecuteMethod;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteMethod != null)
            {
                return canExecuteMethod(parameter);
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            if(CanExecute(parameter))
            {
                method(parameter);
            }
        }
    }
}

