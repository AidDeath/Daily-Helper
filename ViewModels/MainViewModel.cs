
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Models;
using Daily_Helper.Services;
using Daily_Helper.Views;
using System.Collections.ObjectModel;
using System.Linq;

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

            ShowAddRoutineWindowCommand = new RelayCommand(OnShowAddRoutineWindowCommandExecuted);
            
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
            ConnPortRoutine connPort = new("F500-SR-FTP", 21);
            connPort.IsActivated = true;
            Routines.Add(connPort);

        }

        public IRaisedCommand AddNewFreeSpaceCommand { get; }

        private void OnAddNewFreeSpaceCommandExecuted(object obj)
        {
            FileShareRoutine freeSpace = new(@"\\F506-PC2031\");
            Routines.Add(freeSpace);

        }

        public IRaisedCommand ShowAddRoutineWindowCommand { get; }

        private void OnShowAddRoutineWindowCommandExecuted(object obj)
        {
            var wnd = new AddRoutineWindow()
            {
                Owner = GetCurrentWindow()
            };

            var vm = wnd.DataContext as AddRoutineViewModel;

            var result = wnd.ShowDialog();

            if (result == true)
                switch (vm.RoutineType)
                {
                    case Helpers.Enums.RoutineTypes.ConnectToPort:
                        Routines.Add(vm.ConnPortRoutine);
                        break;
                    case Helpers.Enums.RoutineTypes.Ping:
                        Routines.Add(vm.PingRoutine);
                        break;
                    case Helpers.Enums.RoutineTypes.FileShare:
                        vm.FileShareRoutine.WatchedShares = new(vm.AvailableShares.Where(share => share.IsSelected));
                        Routines.Add(vm.FileShareRoutine);
                        break;
                    default:
                        break;
                }


        }


    }
}
