using System;

namespace Daily_Helper.Helpers.Commands
{
    /// <summary>
    /// Универсальная команда для вызова
    /// </summary>
    public class RelayReturnCommand<T> : ReturnCommand<T>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Func<object, T> _execute;

        /// <summary>
        /// 
        /// </summary>
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Конструктор команды
        /// </summary>
        /// <param name="execute">Вызов команды</param>
        /// <param name="canExecute">Проверка доступности команды</param>
        public RelayReturnCommand(Func<object, T> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(Execute));
            _canExecute = canExecute;

        }

        /// <summary>
        /// Вызов команды
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        /// <summary>
        /// Проверка доступности команды
        /// </summary>
        /// <param name="parameter"></param>
        public override T Execute(object parameter) => _execute(parameter);
    }
}
