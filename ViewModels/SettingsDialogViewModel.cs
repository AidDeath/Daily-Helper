using Daily_Helper.Helpers.Commands;
using Daily_Helper.Helpers.Enums;
using Daily_Helper.Models;
using Daily_Helper.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Daily_Helper.ViewModels
{
    public class SettingsDialogViewModel : BaseViewModel
    {
        private readonly SettingsSingleton _settings;
        public SettingsDialogViewModel(SettingsSingleton settings)
        {
            Title = "Настройки";
            _settings = settings;

            CheckInterval = settings.CheckInterval;


            SubmitChangesCommand = new RelayCommand(OnSubmitChangesCommandExecuted, CanSubmitChangesCommandExecute);
        }

        private int checkInterval;
        public int CheckInterval
        {
            get => checkInterval;
            set => SetProperty(ref checkInterval, value);
        }

        public IRaisedCommand SubmitChangesCommand { get; }
        /// <summary>
        /// Close window and return true from dialog
        /// </summary>
        /// <param name="obj"></param>
        private void OnSubmitChangesCommandExecuted(object obj)
        {
            _settings.SetCheckInterval(CheckInterval);
            DialogHost.Close("MaterialSettingsDialogHost", "here i return");
        }

        private bool CanSubmitChangesCommandExecute(object obj)
        {
            return true;
        }
    }
}
