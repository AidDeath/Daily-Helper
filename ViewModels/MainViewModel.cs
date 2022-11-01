
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Models;
using Daily_Helper.Services;
using Daily_Helper.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;

namespace Daily_Helper.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<RoutineBase> _routines;

        public ObservableCollection<RoutineBase> Routines
        {
            get => _routines;
            set
            {
                SetProperty(ref _routines, value);
            }
        }
        public MainViewModel(RoutineTestsProvider routines)
        {
            Title = "Daily Helper";
            Routines = routines.Routines;

            ShowAddRoutineWindowCommand = new RelayCommand(OnShowAddRoutineWindowCommandExecuted);
            ShowSettingsWindowCommand = new RelayCommand(OnShowSettingsWindowCommandExected);
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

            //ПЕРЕДЕЛАТЬ! ВОЗМОЖНЫ УТЕЧКИ ПАМЯТИ?!
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
                    case Helpers.Enums.RoutineTypes.ServiceState:
                        vm.ServiceStateRoutine.WatchedServices = new(vm.AvailableServcies.Where(service => service.IsSelected));
                        Routines.Add(vm.ServiceStateRoutine);
                        break;
                    case Helpers.Enums.RoutineTypes.ProcessState:
                        vm.ProcessStateRoutine.WatchingProcesses = new(vm.AvailableProcesses.Where(proc => proc.IsSelected));
                        Routines.Add(vm.ProcessStateRoutine);
                        break;
                    case Helpers.Enums.RoutineTypes.DriveFreeSpace:
                        Routines.Add(vm.DriveFreeSpaceRoutine);
                        break;
                    default:
                        break;
                }

        }

        public IRaisedCommand ShowSettingsWindowCommand { get; }

        private void OnShowSettingsWindowCommandExected(object obj)
        {
            // FOR TESTS FOR NOW

            var computerName = "F506-SR-DOC";

            EndpointAddress endpoint = new EndpointAddress(@"net.tcp://" + computerName + @":9002/DailyHelperAgent");

            NetTcpBinding binding = new();             

            var client = new AgentServiceClient(binding, endpoint);

            var testValue = client.GetProcessList();
            var testvalue2 = client.GetProcessState("miranda32");
            var testvalue3 = client.GetDrivesFreeSpace();

            Console.WriteLine(testValue + " " + testvalue2);

        }


    }
}
