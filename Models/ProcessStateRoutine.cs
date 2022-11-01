using Daily_Helper.Helpers;
using DailyHelperAgentLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class ProcessStateRoutine : RoutineBase
    {

        private string _server;
        public string Server
        {
            get => _server;
            set => SetProperty(ref _server, value);
        }

        private IEnumerable<string> _allProcesses;

        private ObservableCollection<ProcessInfo>? _watchingProcesses;
        /// <summary>
        /// File shares that would be checking in routine
        /// </summary>
        public ObservableCollection<ProcessInfo>? WatchingProcesses
        {
            get => _watchingProcesses;
            set => SetProperty(ref _watchingProcesses, value);
        }

        public override string Description => $"Состояние процессов на {Server}";

        public override async Task ExecuteRoutineTest()
        {
            var results = new List<string>();
            try
            {
                foreach (var processInfo in WatchingProcesses)
                {
                    RefreshProcessInfo(processInfo);

                    var text = processInfo.IsFound
                        ? processInfo.IsResponding 
                                ? "запущен и отвечает"
                                : "НЕ ОТВЕЧАЕТ"
                        : "закрыт";
                    results.Add($"{processInfo.Name} - {text}");
                }

                Success = true;
                Result = results.Aggregate((a, b) => a + $"\n{b}");
            }
            catch (Exception ex)
            {
                Success = false;
                Result = $"Ошибка: {ex.Message}";
            }

            
        }

        private void RefreshProcessInfo(ProcessInfo processInfo)
        {
            using (var client = new AgentServiceClient(
                new NetTcpBinding(),
                new EndpointAddress(@"net.tcp://" + Server + @":9002/DailyHelperAgent")))
            {
                processInfo = new ProcessInfo(client.GetProcessState(processInfo.Name));
            }

        }
    }
}
