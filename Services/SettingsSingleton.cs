using Daily_Helper.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Daily_Helper.Services
{
    public class SettingsSingleton
    {
        private RegistryKey dailyHelperRegKey;

        //REQUIRES 16 BYTES 
        private readonly string cryptKey = "___KlochkoAG____";

        public int CheckInterval { get; private set; }
        public bool IsTiledViewPreferred { get; private set; }

        public int RememberedWidth { get; private set; }
        public int RememberedHeight { get; private set; }

        public string SmtpServer { get; private set; }
        public string SenderLogin { get; private set; }
        public string SenderPassword { get; private set; }

        public SettingsSingleton()
        {
            
            using (var softwareRegKey = Registry.CurrentUser.OpenSubKey("Software", true))
            {
                if (softwareRegKey is null) dailyHelperRegKey = Registry.CurrentUser.CreateSubKey("DailyHelper");
                else dailyHelperRegKey = softwareRegKey.CreateSubKey("DailyHelper");
            }

#pragma warning disable CS8605 // Default value will return if value is null
            CheckInterval = (int)dailyHelperRegKey.GetValue("CheckInterval", 60);
            IsTiledViewPreferred = bool.Parse((string)dailyHelperRegKey.GetValue("IsTiledViewPreferred", "False"));
            RememberedHeight = (int)dailyHelperRegKey.GetValue("RememberedHeight", 450);
            RememberedWidth = (int)dailyHelperRegKey.GetValue("RememberedWidth", 800);

            SmtpServer = (string)dailyHelperRegKey.GetValue("SmtpServer", "mail.mfrb.by");
            SenderLogin = (string)dailyHelperRegKey.GetValue("SenderLogin", "mail.mfrb.by");
            var tempPassword = (string)dailyHelperRegKey.GetValue("SenderPassword", "");
            SenderPassword = !string.IsNullOrEmpty(tempPassword) ? SimpleCrypter.DecryptString(cryptKey, tempPassword) : tempPassword;

#pragma warning restore CS8605 // Unboxing a possibly null value.
        }

        public void SetCheckInterval(int minutes)
        {
            dailyHelperRegKey.SetValue("CheckInterval", minutes);
#pragma warning disable CS8605 // Unboxing a possibly null value.
            CheckInterval = (int)dailyHelperRegKey.GetValue("CheckInterval", 60);
#pragma warning restore CS8605 // Unboxing a possibly null value.
        }

        public void SetTiledViewPreferrence(bool yesOrNo)
        {
            dailyHelperRegKey.SetValue("IsTiledViewPreferred", yesOrNo);
#pragma warning disable CS8605 // Unboxing a possibly null value.
            IsTiledViewPreferred = bool.Parse((string)dailyHelperRegKey.GetValue("IsTiledViewPreferred", false));
#pragma warning restore CS8605 // Unboxing a possibly null value.
        }

        public void SetWindowSize(int Width, int Height)
        {
            dailyHelperRegKey.SetValue("RememberedWidth", Width);
            dailyHelperRegKey.SetValue("RememberedHeight", Height);
        }

        public void SetMailParameters(string smtpServer, string senderLogin, string senderPassword)
        {
            dailyHelperRegKey.SetValue("SmtpServer", smtpServer);
            dailyHelperRegKey.SetValue("SenderLogin", senderLogin);
            dailyHelperRegKey.SetValue("SenderPassword", SimpleCrypter.EncryptString(cryptKey, senderPassword));

            SmtpServer = smtpServer;
            SenderLogin = senderLogin;
            SenderPassword = senderPassword;
        }

    }
}
