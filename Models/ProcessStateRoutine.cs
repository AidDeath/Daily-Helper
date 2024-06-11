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

        private bool _tryToRerunProcess;
        public bool TryToRerunProcess
        {
            get => _tryToRerunProcess;
            set => SetProperty(ref _tryToRerunProcess, value);
        }

        private IEnumerable<string> _allProcesses;

        private ObservableRangeCollection<ProcessInfo>? _watchingProcesses;
        /// <summary>
        /// File shares that would be checking in routine
        /// </summary>
        public ObservableRangeCollection<ProcessInfo>? WatchingProcesses
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
                RefreshWatchingProcesses();
                foreach (var processInfo in WatchingProcesses)
                {
                    //RefreshProcessInfo(processInfo);

                    var text = processInfo.IsFound
                        ? processInfo.IsResponding 
                                ? "запущен и отвечает"
                                : "НЕ ОТВЕЧАЕТ"
                        : "закрыт";
                    results.Add($"{processInfo.Name} - {text}");

                    if (!string.IsNullOrEmpty(processInfo.FullProcessPath) && text == "закрыт" && TryToRerunProcess) 
                        RerunProcess(processInfo.FullProcessPath);
                }

                Success = WatchingProcesses.All(p => p.IsFound && p.IsResponding);
                Result = results.Aggregate((a, b) => a + $"\n{b}");
            }
            catch (Exception ex)
            {
                Success = false;
                Result = $"Ошибка: {ex.Message}";
            }

            
        }

        private void RefreshWatchingProcesses()
        {
            var RefreshedWatchedProcesses = new ObservableRangeCollection<ProcessInfo>();

            using (var client = new AgentServiceClient(
                new NetTcpBinding(),
                new EndpointAddress(@"net.tcp://" + Server + @":9002/DailyHelperAgent")))
            {
                foreach (var process in WatchingProcesses)
                {
                    RefreshedWatchedProcesses.Add(new ProcessInfo(client.GetProcessState(process.Name)));
                }

            }

            WatchingProcesses.Clear();
            WatchingProcesses.AddRange(RefreshedWatchedProcesses);
            
        }

        private void RerunProcess(string executablePath)
        {
            using (var client = new AgentServiceClient(
            new NetTcpBinding(),
            new EndpointAddress(@"net.tcp://" + Server + @":9002/DailyHelperAgent")))
            {
                client.RunProcess(executablePath);
            }
        }
    }
}
