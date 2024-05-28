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
    public class SelectMailReceiversViewModel : BaseViewModel
    {
        private DailyHelperDbContext _db;
        public SelectMailReceiversViewModel(DailyHelperDbContext db)
        {
            Title = "Выбор адресатов";
            _db = db;

            ExitCommand = new RelayCommand(OnExitCommand);

            Emails = new(_db.Emails);

            EmailSelections = new ObservableRangeCollection<EmailSelection>();

            foreach (var email in Emails) EmailSelections.Add(new EmailSelection() { Email = email, IsSelected = false });

        }

        private ObservableRangeCollection<Email> _emails;
        public ObservableRangeCollection<Email> Emails
        {
            get => _emails;
            set => SetProperty(ref _emails, value);
        }
        

        private ObservableRangeCollection<EmailSelection> _emailSelections;
        public ObservableRangeCollection<EmailSelection> EmailSelections
        {
            get => _emailSelections;
            set => SetProperty(ref _emailSelections, value);
        }

        public IRaisedCommand ExitCommand { get; }
        /// <summary>
        /// close window without saving
        /// </summary>
        /// <param name="obj"></param>
        private void OnExitCommand(object obj)
        {
            DialogHost.Close("SelectMailReceiversDialogHost", EmailSelections.Where(eml => eml.IsSelected).Select(eml => eml.Email));
        }

    }

    public class EmailSelection : ObservableObject
    {

        private Email _email;
        public Email Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => (_isSelected == true);
            set => SetProperty(ref _isSelected, value);
        }
        
    }
}
