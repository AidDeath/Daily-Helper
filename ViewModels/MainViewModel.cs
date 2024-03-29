﻿
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Models;
using Daily_Helper.Services;
using Daily_Helper.Views;
using Daily_Helper.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Daily_Helper.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<RoutineBase> _routines;
        private RoutinesSaveLoadService _saveLoadService;
        private SettingsSingleton _settings;

        public ObservableCollection<RoutineBase> Routines
        {
            get => _routines;
            set => SetProperty(ref _routines, value);
        }
        public MainViewModel(RoutineTestsProvider routines, SettingsSingleton settings, RoutinesSaveLoadService saveLoadService)
        {
            Title = "Daily Helper";
            Routines = routines.Routines;
            _saveLoadService = saveLoadService;
            _settings = settings;
            IsTileView = _settings.IsTiledViewPreferred;
            WindowWidth = _settings.RememberedWidth;
            WindowHeight = _settings.RememberedHeight;

            ShowAddRoutineWindowCommand = new AsyncRelayCommand(OnShowAddRoutineWindowCommandExecuted);
            ShowSettingsWindowCommand = new RelayCommand(OnShowSettingsWindowCommandExected);
            RemoveRoutineCommand = new RelayCommand(OnRemoveRoutineCommandExecuted);
            ChangeViewCommand = new RelayCommand(OnChangeViewCommandExecuted);

            DisableAllRoutinesCommand = new RelayCommand(OnDisableAllRoutinesCommandExecuted, CanDisableAllRoutinesCommandExecute);
            EnableAllRoutinesCommand = new RelayCommand(OnEnableAllRoutinesCommandExecuted, CanEnableAllRoutinesCommandExecute);
            ExportRoutinesCommand = new AsyncRelayCommand(OnExportRoutinesCommandExecuted, CanExportRoutinesCommandExecute);
            ImportRoutinesCommand = new AsyncRelayCommand(OnImportRoutinesCommandExecuted);
            DeleteAllRoutines = new RelayCommand(OnDeleteAllRoutinesExecuted, CanDeleteAllRoutinesExecute);

            RememberWindowSizeCommand = new RelayCommand(OnRememberWindowSizeCommandExecuted);

        }

        private bool _isTileView;
        //If Tiled view selected
        public bool IsTileView
        {
            get => _isTileView;
            set => SetProperty(ref _isTileView, value);
        }

        private int _windowWidth;
        public int WindowWidth
        {
            get => _windowWidth;
            set => SetProperty(ref _windowWidth, value);
        }

        private int _windowHeight;
        public int WindowHeight
        {
            get => _windowHeight;
            set => SetProperty(ref _windowHeight, value);
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
                case Helpers.Enums.RoutineTypes.FolderLastChanged:
                    Routines.Add(vm.FolderLastChangedRoutine);
                    break;
                default:
                    break;
            }

        }

        public IRaisedCommand ShowSettingsWindowCommand { get; }

        private void OnShowSettingsWindowCommandExected(object obj)
        {
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

        public IRaisedCommand ExportRoutinesCommand { get; }

        //Working on 
        private async Task OnExportRoutinesCommandExecuted( object obj)
        {

            var fileDialog = new SaveFileDialog()
            {
                Title = "Экспорт заданий",
                Filter = "Набор заданий|*.dhlist",
                AddExtension = true,
                DefaultExt = "dhlist"
            };

            if (fileDialog.ShowDialog() is true)
            {
                try
                {
                    await _saveLoadService.SaveToFile(fileDialog.FileName);
                }
                catch (Exception ex)
                {
                    await DialogHost.Show(MaterialMessageBox.Create($"Ошибка при экспорте: {ex.GetBaseException().Message}", MessageType.Error), "MaterialMessageBox");
                    //MessageBox.Show(ex.GetBaseException().Message, "Ошибка экспорта", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanExportRoutinesCommandExecute(object obj)
        {
            return (Routines is not null && Routines.Count > 0);
        }

        public IRaisedCommand ImportRoutinesCommand { get; }

        private async Task OnImportRoutinesCommandExecuted(object obj)
        {
            
            var vm = await DialogHost.Show(new LoadFromFileOptionsView(), "LoadFromFileOptionsDialogHost") as LoadFromFileOptionsViewModel;

            if (vm is null) return;

            //If we want to overwrite previous - we need to clear collection
            if (vm.OverwriteRoutinesSelected)
            {
                OnDisableAllRoutinesCommandExecuted(new object());
                Routines.Clear();
            }

            foreach (var addingRoutine in vm.SelectedRoutines)
                Routines.Add(addingRoutine);


        }

        public IRaisedCommand RememberWindowSizeCommand { get; }
        private void OnRememberWindowSizeCommandExecuted(object obj)
        {
            _settings.SetWindowSize(WindowWidth, WindowHeight);
        }

        public IRaisedCommand DeleteAllRoutines { get; }

        private void OnDeleteAllRoutinesExecuted(object obj)
        {
            OnDisableAllRoutinesCommandExecuted(new object());
            Routines.Clear();
        }

        private bool CanDeleteAllRoutinesExecute(object obj)
        {
            return Routines.Count > 0;
        }


    }
}
