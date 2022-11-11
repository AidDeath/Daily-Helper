using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Services
{
    public class SettingsSingleton
    {
        private RegistryKey dailyHelperRegKey;

        public int CheckInterval { get; private set; }

        public SettingsSingleton()
        {
            
            using (var softwareRegKey = Registry.CurrentUser.OpenSubKey("Software", true))
            {
                if (softwareRegKey is null) dailyHelperRegKey = Registry.CurrentUser.CreateSubKey("DailyHelper");
                else dailyHelperRegKey = softwareRegKey.CreateSubKey("DailyHelper");
            }

#pragma warning disable CS8605 // Default value will return if value is null
            CheckInterval = (int)dailyHelperRegKey.GetValue("CheckInterval", 60);
#pragma warning restore CS8605 // Unboxing a possibly null value.
        }

        public void SetCheckInterval(int minutes)
        {
            dailyHelperRegKey.SetValue("CheckInterval", minutes);
#pragma warning disable CS8605 // Unboxing a possibly null value.
            CheckInterval = (int)dailyHelperRegKey.GetValue("CheckInterval", 60);
#pragma warning restore CS8605 // Unboxing a possibly null value.
        }
    }
}
