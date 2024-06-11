using Daily_Helper.Helpers;
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.ViewModels
{
    public class MailLogViewModel : BaseViewModel
    {
        private DailyHelperDbContext _db;
        public MailLogViewModel(DailyHelperDbContext db)
        {
            _db = db;

            MailLogs = new (
                _db.MailLogs
                .OrderByDescending(ml => ml.SendAt)
                .Include(ml => ml.Email));

            ExitCommand = new RelayCommand(OnExitCommand);
        }

        private ObservableRangeCollection<MailLog> _mailLogs;
        public ObservableRangeCollection<MailLog> MailLogs
        {
            get => _mailLogs;
            set => SetProperty(ref _mailLogs, value); 
        }


        public IRaisedCommand ExitCommand { get; }
        /// <summary>
        /// close window without saving
        /// </summary>
        /// <param name="obj"></param>
        private void OnExitCommand(object obj)
        {
            DialogHost.Close("MailLogDialogHost");
        }

    }
}
