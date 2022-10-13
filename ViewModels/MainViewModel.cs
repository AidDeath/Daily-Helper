
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
