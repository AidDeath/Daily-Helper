using DailyHelperAgentLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class ProcessInfo : ObservableObject
    {
        public ProcessInfo()
        {

        }

        public ProcessInfo(ProcessState processState)
        {
            Name = processState.Name;
            IsFound = processState.IsFound;
            IsResponding = processState.IsResponding;
        }

        private bool _isSelected;
        /// <summary>
        /// Is this service selected in list
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }


        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _isFound;
        public bool IsFound
        {
            get => _isFound;
            set => SetProperty(ref _isFound, value);
        }

        private bool _isResponding;
        public bool IsResponding
        {
            get => _isResponding;
            set => SetProperty(ref _isResponding, value);
        }

        public static IEnumerable<ProcessInfo> GetAllProcessInfo(string computerName)
        {
            var processInfoList = new List<ProcessInfo>();

            using (var client = new ProcessServiceClient(
                new NetTcpBinding(),
                new EndpointAddress(@"net.tcp://" + computerName + @":9002/DailyHelperAgent")))
            {
                var processes = client.GetProcessList();
                foreach (var process in processes)
                {
                    processInfoList.Add(new ProcessInfo(client.GetProcessState(process)));
                }
            }

            return processInfoList;
        }

    }
}
