
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Models;
using Daily_Helper.Services;
using Daily_Helper.Views;
using Daily_Helper.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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
        public MainViewModel(RoutineTestsProvider routines, SettingsSingleton settings)
        {
            Title = "Daily Helper";
            Routines = routines.Routines;
            IsTileView = settings.IsTiledViewPreferred;

            ShowAddRoutineWindowCommand = new AsyncRelayCommand(OnShowAddRoutineWindowCommandExecuted);
            ShowSettingsWindowCommand = new RelayCommand(OnShowSettingsWindowCommandExected);
            RemoveRoutineCommand = new RelayCommand(OnRemoveRoutineCommandExecuted);
            ChangeViewCommand = new RelayCommand(OnChangeViewCommandExecuted);

            DisableAllRoutinesCommand = new RelayCommand(OnDisableAllRoutinesCommandExecuted, CanDisableAllRoutinesCommandExecute);
            EnableAllRoutinesCommand = new RelayCommand(OnEnableAllRoutinesCommandExecuted, CanEnableAllRoutinesCommandExecute);
        }

        private bool _isTileView;
        //If Tiled view selected
        public bool IsTileView
        {
            get => _isTileView;
            set => SetProperty(ref _isTileView, value);
        }

        public IRaisedCommand ShowAddRoutineWindowCommand { get; }

        private async Task OnShowAddRoutineWindowCommandExecuted(object obj)
        {
            var vm = await DialogHost.Show(new AddRoutineWindow(), "AddRoutinesDialogHost") as AddRoutineViewModel;

            if (vm is null) return; //add routine cancelled

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

            //var wnd = new SettingsWindow()
            //{
            //    Owner = GetCurrentWindow()
            //};

            //var vm = wnd.DataContext as SettingsViewModel;

            //var result = wnd.ShowDialog();

            DialogHost.Show(new SettingsDialogView(), "MaterialSettingsDialogHost");
        }

        
        public IRaisedCommand RemoveRoutineCommand { get; }

        private void OnRemoveRoutineCommandExecuted(object obj)
        {
            if (obj is not null)
                Routines.Remove(obj as RoutineBase);
        }



        public IRaisedCommand DisableAllRoutinesCommand { get; }

        private bool CanDisableAllRoutinesCommandExecute(object obj)
        {
            return Routines.Any(r => r.IsActivated);
        }

        private void OnDisableAllRoutinesCommandExecuted(object obj)
        {
            foreach (var routine in Routines)
            {
                routine.IsActivated = false;
            }
        }

        public IRaisedCommand EnableAllRoutinesCommand { get; }

        private bool CanEnableAllRoutinesCommandExecute(object obj)
        {
            return !Routines.All(r => r.IsActivated);
        }

        private void OnEnableAllRoutinesCommandExecuted(object obj)
        {
            foreach (var routine in Routines)
            {
                routine.IsActivated = true;
            }
        }
        public IRaisedCommand ChangeViewCommand { get; }

        private void OnChangeViewCommandExecuted(object obj)
        {
            IsTileView = !IsTileView;
        }


    }
}
