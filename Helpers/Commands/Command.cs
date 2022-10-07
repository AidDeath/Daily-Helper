using System;
using System.Windows.Input;

namespace Daily_Helper.Helpers.Commands
{
    /// <summary>
    /// Абстрактная команда
    /// </summary>
    public abstract class Command : IRaisedCommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void RaiseCanExecuteChanged()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(CommandManager.InvalidateRequerySuggested);
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
