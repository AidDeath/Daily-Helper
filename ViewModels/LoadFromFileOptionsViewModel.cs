using Daily_Helper.Helpers;
using Daily_Helper.Helpers.Commands;
using Daily_Helper.Models;
using Daily_Helper.Services;
using Daily_Helper.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Daily_Helper.ViewModels
{
    public class LoadFromFileOptionsViewModel : BaseViewModel
    {
        private readonly RoutinesSaveLoadService _saveLoadService;
        public LoadFromFileOptionsViewModel(RoutinesSaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;

           OpenFileCommand = new AsyncRelayCommand(OnOpenFileCommandExecuted);

            LoadRoutinesCommand = new RelayCommand(OnLoadRoutinesCommandExecuted);
            CloseCommand = new RelayCommand(OnCloseCommandExecuted);
        }

        //True if overwrite routines picked, false otherwise
        public bool OverwriteRoutinesSelected { get; set; }

        private bool _isFileSelected;
        public bool IsFileSelected
        {
            get => _isFileSelected;
            set => SetProperty(ref _isFileSelected, value);
        }

        private ObservableRangeCollection<AddingRoutine> _addingRoutines = new ObservableRangeCollection<AddingRoutine>();
        public ObservableRangeCollection<AddingRoutine> AddingRoutines
        {
            get => _addingRoutines;
            set => SetProperty(ref _addingRoutines, value);
        }

        public IEnumerable<RoutineBase> SelectedRoutines => AddingRoutines.Where(r => r.IsSelected).Select(r => r.Routine);


        public IRaisedCommand LoadRoutinesCommand { get; }
        private void OnLoadRoutinesCommandExecuted(object obj)
        {
            OverwriteRoutinesSelected = bool.Parse((string)obj);

            DialogHost.Close("LoadFromFileOptionsDialogHost", this);
        }


        public IRaisedCommand CloseCommand { get; }
        private void OnCloseCommandExecuted(object obj)
        {
            DialogHost.Close("LoadFromFileOptionsDialogHost", null);
        }


        public IRaisedCommand OpenFileCommand { get; }
        private async Task OnOpenFileCommandExecuted(object obj)
        {
            var fileDialog = new OpenFileDialog()
            {
                Filter = "Набор заданий|*.dat",
                Title = "Импорт заданий",
            };

            if (fileDialog.ShowDialog() != true) return;

            try
            {
                var routinesFromFile = await _saveLoadService.LoadFromFile(fileDialog.FileName);
                if (routinesFromFile is null) throw new Exception("В выбранном файле нет задач");

                foreach (var routine in routinesFromFile)
                    if (routine?.Type is not null && routine.JsonString is not null)
                    {
                        AddingRoutines.Add(new AddingRoutine 
                        { Routine = (RoutineBase)JsonSerializer.Deserialize(routine.JsonString, routine.Type)} );
                    }
            }
            catch (Exception e)
            {
                await DialogHost.Show(MaterialMessageBox.Create($"Ошибка: {e.GetBaseException().Message}", MessageType.Error), "MaterialMessageBox");
            }

            IsFileSelected = true;

        }

        public class AddingRoutine : ObservableObject
        {
            private bool _isSelected = true;
            public bool IsSelected
            {
                get => _isSelected;
                set => SetProperty(ref _isSelected, value);
            }

            private RoutineBase _routine;
            public RoutineBase Routine 
            { 
                get => _routine;
                set => SetProperty(ref _routine, value); 
            }
        }


    }
}
