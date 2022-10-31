using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace DailyHelperAgentService
{
    [RunInstaller(true)]
    public partial class ServiceInstaller : System.Configuration.Install.Installer
    {
        public ServiceInstaller()
        {
            // InitializeComponent();
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;

            service = new System.ServiceProcess.ServiceInstaller();
            service.ServiceName = "DailyHelperAgent";
            service.DisplayName = "Daily Helper Agent";
            service.Description = "Daily Helper Agent for running routines at this machine remotely from Daily Helper";
            service.StartType = ServiceStartMode.Automatic;

            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
