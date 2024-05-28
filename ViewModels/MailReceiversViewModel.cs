using Daily_Helper.Helpers;
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Models;
using Daily_Helper.Views;
using Daily_Helper.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Daily_Helper.ViewModels
{
    public class MailReceiversViewModel : BaseViewModel
    {
        private DailyHelperDbContext _db;
        public MailReceiversViewModel(DailyHelperDbContext db)
        {
            _db = db;
            Title = "Адресаты уведомлений";
            ChangeReceiversCommand = new AsyncRelayCommand(OnChangeReceiversCommandExecuted);
            ExitCommand = new RelayCommand(OnExitCommand);

            RoutineIdentifers  = new ObservableRangeCollection<RoutineIdentifer>(_db.RoutineIdentifers.Include(ri => ri.MailRecievers));


        }

        private ObservableRangeCollection<MailReciever> _mailReceivers;
        public ObservableRangeCollection<MailReciever> MailReceivers
        {
            get => _mailReceivers;
            set => SetProperty(ref _mailReceivers, value);
        }

        private ObservableRangeCollection<RoutineIdentifer> _routineIdentifers;
        public ObservableRangeCollection<RoutineIdentifer> RoutineIdentifers
        {
            get => _routineIdentifers;
            set => SetProperty(ref _routineIdentifers, value);
        }


        public IRaisedCommand ExitCommand { get; }
        /// <summary>
        /// close window without saving
        /// </summary>
        /// <param name="obj"></param>
        private void OnExitCommand(object obj)
        {
            DialogHost.Close("MailReceiversDialogHost");
        }

        public IRaisedCommand ChangeReceiversCommand { get; }

        private async Task OnChangeReceiversCommandExecuted(object obj)
        {
            var routineIdentifer = obj as RoutineIdentifer; 
            var emails = await DialogHost.Show(new SelectMailReceiversView(), "SelectMailReceiversDialogHost") as IEnumerable<Email>;
            
            routineIdentifer.MailRecievers.Clear();
            foreach (var email in emails)
            {
                routineIdentifer.MailRecievers.Add(new MailReciever { EmailAddressId = email.Id, Email = email, RoutineIdentifer = routineIdentifer });
            }
            
            _db.SaveChanges();
        }
    }
}
