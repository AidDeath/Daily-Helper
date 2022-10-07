using System;
using System.Windows.Input;

namespace Daily_Helper.Helpers.Commands
{
    /// <summary>
    /// Абстрактная команда
    /// </summary>
    public abstract class ReturnCommand<T> : IRaisedCommand
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

        public abstract T Execute(object parameter);

        public abstract bool CanExecute(object parameter);

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute(parameter);
        }
    }
}
