using System;
using System.Globalization;

namespace Daily_Helper.Helpers.Commands
{
    /// <summary>
    /// Команда действия для событий
    /// </summary>
    /// <typeparam name="T">Тип события</typeparam>
    public class ActionCommand<T> : Command
    {
        // public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 
        /// </summary>
        private readonly Action<T> _action;

        /// <summary>
        /// Конструктор действия
        /// </summary>
        /// <param name="action">Действие</param>
        public ActionCommand(Action<T> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(Execute));
        }

        /// <summary>
        /// Доступность действия
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object parameter) => true;

        /// <summary>
        /// Вызов действия
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if (_action == null) return;
            var castParameter = Convert.ChangeType(parameter, typeof(T), CultureInfo.InvariantCulture);
            _action((T)castParameter);
        }
    }
}
