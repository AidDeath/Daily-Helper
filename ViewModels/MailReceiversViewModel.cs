using Daily_Helper.Helpers;
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.ViewModels
{
    public class MailReceiversViewModel : BaseViewModel
    {
        private DailyHelperDbContext _db;
        public MailReceiversViewModel(DailyHelperDbContext db)
        {
            _db = db;
            Title = "Адресаты уведомлений";
            ExitCommand = new RelayCommand(OnExitCommand);
        }

        private ObservableRangeCollection<MailReciever> _mailReceivers;




        public IRaisedCommand ExitCommand { get; }
        /// <summary>
        /// close window without saving
        /// </summary>
        /// <param name="obj"></param>
        private void OnExitCommand(object obj)
        {
            DialogHost.Close("MailReceiversDialogHost");
        }
    }
}
