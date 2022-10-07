using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Daily_Helper.Helpers
{
    /// <summary>
    /// Hepler for UI stream syncronize
    /// </summary>
    public static class DispatcherHelper
    {
        public static Dispatcher UiDispatcher
        {
            get;
            private set;
        }

        public static async Task CheckBeginInvokeOnUIAsync(Action action)
        {
            if (UiDispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                await UiDispatcher.InvokeAsync(action);
            }
        }

        public static void CheckBeginInvokeOnUI(Action action)
        {
            if (UiDispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                UiDispatcher.BeginInvoke(action);
            }
        }

        public static void Initialize()
        {
            if (UiDispatcher != null)
            {
                return;
            }

            UiDispatcher = Dispatcher.CurrentDispatcher;
        }
    }
}