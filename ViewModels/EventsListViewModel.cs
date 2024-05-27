using Daily_Helper.Helpers.Commands;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.ViewModels
{
    public class EventsListViewModel : BaseViewModel
    {
        public EventsListViewModel()
        {
            Title = "Просмотр событий";

            ExitCommand = new RelayCommand(OnExitCommand);

        }
        public IRaisedCommand ExitCommand { get; }
        /// <summary>
        /// close window without saving
        /// </summary>
        /// <param name="obj"></param>
        private void OnExitCommand(object obj)
        {
            DialogHost.Close("EventsListDialogHost");
        }
    }
}
