using System;

namespace Daily_Helper.Helpers.Commands
{
    /// <summary>
    /// Универсальная команда для вызова
    /// </summary>
    public class RelayCommand : Command
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// 
        /// </summary>
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Конструктор команды
        /// </summary>
        /// <param name="execute">Вызов команды</param>
        /// <param name="canExecute">Проверка доступности команды</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
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
        public override void Execute(object parameter) => _execute(parameter);
    }

    /// <summary>
    /// Универсальная команда для вызова
    /// </summary>
    /// <typeparam name="T">Тип параметра команды</typeparam>
    public class RelayCommand<T> : Command
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Action<T> _execute;

        /// <summary>
        /// 
        /// </summary>
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// Конструктор команды
        /// </summary>
        /// <param name="execute">Вызов команды</param>
        /// <param name="canExecute">Проверка доступности команды</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;

        }

        /// <summary>
        /// Вызов команды
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object parameter)
        {
            return parameter != null && (_canExecute?.Invoke((T)parameter) ?? true);
        }

        /// <summary>
        /// Проверка доступности команды
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter) => _execute((T)parameter);
    }
}
