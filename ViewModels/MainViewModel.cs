
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Models;
using Daily_Helper.Services;
using System.Collections.ObjectModel;

namespace Daily_Helper.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<RoutineBase> _routines;

        public ObservableCollection<RoutineBase> Routines
        {
            get => _routines;
            set => SetProperty(ref _routines, value);
        }
        public MainViewModel(RoutineTestsProvider routines)
        {
            Title = "Daily Helper";
            Routines = routines.Routines;
            
            PingRoutine ping = new PingRoutine("192.168.3.1");
            ping.IsActivated = true;

            Routines.Add(ping);

            AddNewPingCommand = new RelayCommand(OnAddPingCommandExecuted);
            AddNewConnPortCommand = new RelayCommand(OnAddConnPortCommandExecuted);
            AddNewFreeSpaceCommand = new RelayCommand(OnAddNewFreeSpaceCommandExecuted);
            
        }


        public IRaisedCommand AddNewPingCommand { get; }

        private void OnAddPingCommandExecuted(object obj)
        {
            PingRoutine ping = new("mail.mfrb.by");
            ping.IsActivated = true;
            Routines.Add(ping);

        }


        public IRaisedCommand AddNewConnPortCommand { get; }

        private void OnAddConnPortCommandExecuted(object obj)
        {
            ConnPortRoutine connPort = new("F500-SR-FTPg", 21);
            connPort.IsActivated = true;
            Routines.Add(connPort);

        }

        public IRaisedCommand AddNewFreeSpaceCommand { get; }

        private void OnAddNewFreeSpaceCommandExecuted(object obj)
        {
            FileShareRoutine freeSpace = new(@"\\F506-PC2031\");
            Routines.Add(freeSpace);

        }


    }
}
