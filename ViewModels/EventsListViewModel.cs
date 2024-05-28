using Daily_Helper.Helpers;
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.ViewModels
{
    public class EventsListViewModel : BaseViewModel
    {
        private DailyHelperDbContext _db;
        public EventsListViewModel(DailyHelperDbContext db)
        {
            _db = db;

            Title = "Просмотр событий";

            ExitCommand = new RelayCommand(OnExitCommand);
            FailureEvents = new ObservableRangeCollection<FailureEvent>(_db.FailureEvents.Include(fe => fe.RoutineIdentifer)
                .OrderByDescending(fe=> fe.RoutineIdentifer.IsCurrentlyInList)
                .ThenByDescending(fe=> fe.IsStillActive)
                .ThenByDescending(fe=> fe.Occured));

        }

        private ObservableRangeCollection<FailureEvent> _failureEvents;

        public ObservableRangeCollection<FailureEvent> FailureEvents
        {
            get => _failureEvents;
            set => SetProperty(ref _failureEvents, value);
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
