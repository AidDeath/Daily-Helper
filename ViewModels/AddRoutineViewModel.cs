using Daily_Helper.Helpers.Commands;
using Daily_Helper.Helpers.Enums;
using Daily_Helper.Models;
using System;
using System.Collections.Generic;
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
        /// Drscriptions for a routine tasks
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
            SubmitChangesCommand = new RelayCommand(OnSubmitChangesCommandExecuted);
            
        }

        private RoutineTypes _routineType;
        /// <summary>
        /// Selected in a window routine type
        /// </summary>
        public RoutineTypes RoutineType
        {
            get => _routineType;
            set => SetProperty(ref _routineType, value);
        }


        public IRaisedCommand SubmitChangesCommand { get; }

        private void OnSubmitChangesCommandExecuted(object obj)
        {
            var wnd = GetCurrentWindow();
            wnd.DialogResult = true;
            wnd.Close();
        }

    }
}
