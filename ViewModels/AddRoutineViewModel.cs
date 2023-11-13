using Daily_Helper.Helpers;
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Helpers.Enums;
using Daily_Helper.Models;
using Daily_Helper.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;

namespace Daily_Helper.ViewModels
{

    public class AddRoutineViewModel : BaseViewModel
    {
        private Dictionary<RoutineTypes, string>? _descriptions;
        /// <summary>
        /// Drscriptions for portNo routine tasks
        /// </summary>
        public Dictionary<RoutineTypes, string>? Descriptions
        {
            get => _descriptions;
            set => SetProperty(ref _descriptions, value);
        }

        public AddRoutineViewModel()
        {
            Title = "Добавить задачу проверки";
            Descriptions = RoutinesDescriptions.Descriptions;

            ConnPortRoutine = new("localhost", 80);
            PingRoutine = new("localhost");
            FileShareRoutine = new();
            ServiceStateRoutine = new();
            ProcessStateRoutine = new();
            DriveFreeSpaceRoutine = new("localhost", 10);
            FolderLastChangedRoutine = new();
            
            
            

            SubmitChangesCommand = new RelayCommand(OnSubmitChangesCommandExecuted, CanSubmitChangesCommandExecute);
            CancelAddingRoutineCommand = new RelayCommand(OnCancelAddingRoutineCommand);
            SelectSharesCommand = new RelayCommand(OnSelectSharesCommandExecuted, CanSelectSharesCommandExecute);
            SelectServicesCommand = new RelayCommand(OnSelectServicesCommandExecuted, CanSelectServicesCommandExecute);
            SelectProcessCommand = new RelayCommand(OnSelectProcessCommandExecuted, CanSelectProcessCommandExecute);

            SetPortCommand = new RelayCommand(OnSetPortCommandExecuted);
            
        }

        private RoutineTypes _routineType;
        /// <summary>
        /// Selected in portNo window routine type
        /// </summary>
        public RoutineTypes RoutineType
        {
            get => _routineType;
            set => SetProperty(ref _routineType, value);
        }


        private PingRoutine _pingRoutine;

        public PingRoutine PingRoutine
        {
            get => _pingRoutine;
            set => SetProperty(ref _pingRoutine, value);
        }

        private FileShareRoutine _fileShareRoutine;

        public FileShareRoutine FileShareRoutine
        {
            get => _fileShareRoutine;
            set => SetProperty(ref _fileShareRoutine, value);
        }

        private bool _isSharesSelecting;
        public bool IsSharesSelecting
        {
            get => _isSharesSelecting;
            set => SetProperty(ref _isSharesSelecting, value);
        }

        private ObservableCollection<Share> _availableShares;
        public ObservableCollection<Share> AvailableShares
        {
            get => _availableShares;
            set => SetProperty(ref _availableShares, value);
        }

        #region Servces State

        private ServiceStateRoutine _serviceStateRoutine;
        public ServiceStateRoutine ServiceStateRoutine
        {
            get => _serviceStateRoutine;
            set => SetProperty(ref _serviceStateRoutine, value);
        }

        private bool _isServiceSelecting;
        public bool IsServiceSelecting
        {
            get => _isServiceSelecting;
            set => SetProperty(ref _isServiceSelecting, value);
        }

        private ObservableCollection<ServiceInfo> _availableServcies;

        public ObservableCollection<ServiceInfo> AvailableServcies
        {
            get => _availableServcies;
            set => SetProperty(ref _availableServcies, value);
        }

        #endregion

        #region Processes State

        private ProcessStateRoutine _processStateRoutine;
        public ProcessStateRoutine ProcessStateRoutine
        {
            get => _processStateRoutine;
            set => SetProperty(ref _processStateRoutine, value);
        }

        private bool _isProcessSelecting;
        public bool IsProcessSelecting
        {
            get => _isProcessSelecting;
            set => SetProperty(ref _isProcessSelecting, value);
        }

        private ObservableCollection<ProcessInfo> _availableProcesses;
        public ObservableCollection<ProcessInfo> AvailableProcesses
        {
            get => _availableProcesses;
            set => SetProperty(ref _availableProcesses, value);
        }

        #endregion

        private ConnPortRoutine _connPortRoutine;
        public ConnPortRoutine ConnPortRoutine
        {
            get => _connPortRoutine;
            set => SetProperty(ref _connPortRoutine, value);
        }

        private DriveFreeSpaceRoutine driveFreeSpaceRoutine;
        public DriveFreeSpaceRoutine DriveFreeSpaceRoutine
        {
            get => driveFreeSpaceRoutine;
            set => SetProperty(ref driveFreeSpaceRoutine, value);
        }

        private FolderLastChangedRoutine _folderLastChangedRoutine;
        public FolderLastChangedRoutine FolderLastChangedRoutine
        {
            get => _folderLastChangedRoutine;
            set => SetProperty(ref _folderLastChangedRoutine, value);
        }



        public IRaisedCommand CancelAddingRoutineCommand { get; }
        /// <summary>
        /// close window without saving
        /// </summary>
        /// <param name="obj"></param>
        private void OnCancelAddingRoutineCommand(object obj)
        {
            DialogHost.Close("AddRoutinesDialogHost");
        }


        public IRaisedCommand SubmitChangesCommand { get; }
        /// <summary>
        /// Close window and return true from dialog
        /// </summary>
        /// <param name="obj"></param>
        private void OnSubmitChangesCommandExecuted(object obj)
        {
            //var wnd = GetCurrentWindow();
            //wnd.DialogResult = true;
            //wnd.Close();

            DialogHost.Close("AddRoutinesDialogHost", this);
        }

        private bool CanSubmitChangesCommandExecute(object obj)
        {
            switch (RoutineType)
            {
                case RoutineTypes.Ping:
                    return !PingRoutine.HasErrors; //!string.IsNullOrWhiteSpace(PingRoutine.Hostname) ;
                case RoutineTypes.FileShare:
                    return IsSharesSelecting && AvailableShares.Any(share => share.IsSelected);
                case RoutineTypes.ServiceState:
                    return IsServiceSelecting && AvailableServcies.Any(service => service.IsSelected);
                case RoutineTypes.ProcessState:
                    return IsProcessSelecting && AvailableProcesses.Any(proc => proc.IsSelected);
                case RoutineTypes.ConnectToPort:
                    return !ConnPortRoutine.HasErrors && !string.IsNullOrWhiteSpace(ConnPortRoutine.Hostname) && ConnPortRoutine.Port != 0;
                case RoutineTypes.DriveFreeSpace:
                    return !DriveFreeSpaceRoutine.HasErrors;
                case RoutineTypes.FolderLastChanged:
                    return !FolderLastChangedRoutine.HasErrors;
                default:
                    return false;
            }
        }

        public IRaisedCommand SelectSharesCommand { get; }
        /// <summary>
        /// User inputted portNo file server name and pressed "select shares"
        /// </summary>
        /// <param name="obj"></param>
        private void  OnSelectSharesCommandExecuted(object obj)
        {
            try
            {
                if (IsSharesSelecting == false)
                    AvailableShares = new(ShareDetector.GetAllShares(FileShareRoutine.Server).Where(sh => sh.IsFileSystem));

                IsSharesSelecting = !IsSharesSelecting;
            }
            catch (Exception e)
            {
                DialogHost.Show(MaterialMessageBox.Create($"Ошибка: {e.GetBaseException().Message}", MessageType.Error), "MaterialMessageBox") ;
            }

        }

        private bool CanSelectSharesCommandExecute(object obj)
        {
            return !string.IsNullOrWhiteSpace(FileShareRoutine.Server);
        }


        public IRaisedCommand SelectServicesCommand { get; }
        private void OnSelectServicesCommandExecuted(object obj)
        {
            try
            {
                if (IsServiceSelecting == false)
                    AvailableServcies = new(ServiceInfo.GetAllServices(ServiceStateRoutine.Server));

                IsServiceSelecting = !IsServiceSelecting;
            }
            catch (Exception e)
            {
                DialogHost.Show(MaterialMessageBox.Create($"Ошибка: {e.GetBaseException().Message}", MessageType.Error), "MaterialMessageBox");
            }

        }
        private bool CanSelectServicesCommandExecute(object obj)
        {
            var a = !string.IsNullOrWhiteSpace(ServiceStateRoutine.Server);
            return !string.IsNullOrWhiteSpace(ServiceStateRoutine.Server);
        }

        public IRaisedCommand SelectProcessCommand { get; }
        private void OnSelectProcessCommandExecuted(object obj)
        {
            try
            {
                if (IsProcessSelecting == false)
                    AvailableProcesses = new(ProcessInfo.GetAllProcessInfo(ProcessStateRoutine.Server));

                IsProcessSelecting = !IsProcessSelecting;
            }
            catch (CommunicationObjectFaultedException)
            {
                DialogHost.Show(MaterialMessageBox.Create($"Ошибка: \n Не найден компьютер с таким именем, либо не запущен Daily Helper Agent", MessageType.Error), "MaterialMessageBox");
            }
            catch (Exception e)
            {
                DialogHost.Show(MaterialMessageBox.Create($"Ошибка: {e.GetBaseException().Message}", MessageType.Error), "MaterialMessageBox");
            }
        }
        private bool CanSelectProcessCommandExecute(object obj)
        {
            return !string.IsNullOrWhiteSpace(ProcessStateRoutine.Server);
        }


        public IRaisedCommand SetPortCommand { get; }
        /// <summary>
        /// Sets port at ConnPortRoutineCreation to int value from command parameter
        /// </summary>
        /// <param name="obj"></param>
        private void OnSetPortCommandExecuted (object obj)
        {
            var portNo = int.Parse((string)obj);
            ConnPortRoutine.Port = portNo;
        }

    }
}
