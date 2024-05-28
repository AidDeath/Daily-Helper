using Daily_Helper.Helpers;
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Models;
using MaterialDesignThemes.Wpf;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.ViewModels
{
    public class MailBookViewModel : BaseViewModel
    {
        private DailyHelperDbContext _db;
        public MailBookViewModel(DailyHelperDbContext db)
        {
            _db = db;
            Title = "Справочник получателей";
            ExitCommand = new RelayCommand(OnExitCommand);
            MailAddresses = new ObservableRangeCollection<Email>(_db.Emails);
        }

        private ObservableRangeCollection<Email> _mailAddresses;
        public ObservableRangeCollection<Email> MailAddresses
        {
            get => _mailAddresses;
            set => SetProperty(ref _mailAddresses, value);
        }


        public IRaisedCommand ExitCommand { get; }
        /// <summary>
        /// close window without saving
        /// </summary>
        /// <param name="obj"></param>
        private void OnExitCommand(object obj)
        {
            DialogHost.Close("MailBookDialogHost");
        }
    }
}
