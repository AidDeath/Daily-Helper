
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Models;
using Daily_Helper.Services;
using Daily_Helper.Views;
using Daily_Helper.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Text.Json;
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
            ChangeViewCommand = new RelayCommand(OnChangeViewCommandExecuted);
            RemoveRoutineCommand = new RelayCommand(OnRemoveRoutineCommandExecuted);

            ShowLogsCommand = new RelayCommand(OnShowLogsCommandExecuted);
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



        public IRaisedCommand ShowLogsCommand { get; }

        private void OnShowLogsCommandExecuted(object obj)
        {
            //FOR TESTS

            //DataContractJsonSerializerSettings asd = new() { EmitTypeInformation = System.Runtime.Serialization.EmitTypeInformation.Always };

            var serializedRoutine = _routines.FirstOrDefault()?.GetSerialized();

            if (serializedRoutine?.Type is not null && serializedRoutine.JsonString is not null)
            {
                var a = JsonSerializer.Deserialize(serializedRoutine.JsonString, serializedRoutine.Type);
            }
            
        }

        public IRaisedCommand ChangeViewCommand { get; }

        private void OnChangeViewCommandExecuted(object obj)
        {
            IsTileView = !IsTileView;
        }
    }
}
