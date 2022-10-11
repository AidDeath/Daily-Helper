using Daily_Helper.Helpers;
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Helpers.Enums;
using Daily_Helper.Models;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

            ConnPortRoutine = new(string.Empty, 80);
            PingRoutine = new(string.Empty);
            FileShareRoutine = new(string.Empty);

            SubmitChangesCommand = new RelayCommand(OnSubmitChangesCommandExecuted);
            SelectSharesCommand = new RelayCommand(OnSelectSharesCommandExecuted);

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

        private ConnPortRoutine _connPortRoutine;
        public ConnPortRoutine ConnPortRoutine
        {
            get => _connPortRoutine;
            set => SetProperty(ref _connPortRoutine, value);
        }


        public IRaisedCommand SubmitChangesCommand { get; }
        /// <summary>
        /// Close window and return true from dialog
        /// </summary>
        /// <param name="obj"></param>
        private void OnSubmitChangesCommandExecuted(object obj)
        {
            var wnd = GetCurrentWindow();
            wnd.DialogResult = true;
            wnd.Close();
        }

        public IRaisedCommand SelectSharesCommand { get; }
        /// <summary>
        /// User inputted portNo file server name and pressed "select shares"
        /// </summary>
        /// <param name="obj"></param>
        private void OnSelectSharesCommandExecuted(object obj)
        {
            try
            {
                if (IsSharesSelecting == false)
                    AvailableShares = new(ShareDetector.GetAllShares(FileShareRoutine.Server).Where(sh => sh.IsFileSystem));

                IsSharesSelecting = !IsSharesSelecting;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка: {e.GetBaseException().Message}", "ЗАМЕНМИТЬ НА НОРМАЛЬНОЕ СООБЩЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                //DialogHost.Show($"Ошибка: {e.GetBaseException().Message}");
            }

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
