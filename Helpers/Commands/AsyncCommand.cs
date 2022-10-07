using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Daily_Helper.Helpers.Commands
{
    /// <summary>
    /// Абстрактная команда
    /// </summary>
    public abstract class AsyncCommand : IRaisedCommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void RaiseCanExecuteChanged()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(CommandManager.InvalidateRequerySuggested);
        }

        public abstract Task ExecuteAsync(object parameter);

        public abstract bool CanExecute(object parameter);

        bool ICommand.CanExecute(object? parameter)
        {
            return CanExecute(parameter);
        }

        async void ICommand.Execute(object? parameter)
        {
            await ExecuteAsync(parameter);
        }
    }
}
