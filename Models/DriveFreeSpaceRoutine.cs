using DailyHelperAgentLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class DriveFreeSpaceRoutine : RoutineBase
    {
        private string _server;
        [MinLength(1, ErrorMessage = "Укажите хост")]
        public string Server
        {
            get => _server;
            set => SetProperty(ref _server, value);
        }

        public DriveFreeSpaceRoutine(string hostname)
        {
            Server = hostname;
        }

        private DriveFreeSpace[] drivesFreeSpace;
        public DriveFreeSpace[] DrivesFreeSpace
        {
            get => drivesFreeSpace;
            set => SetProperty(ref drivesFreeSpace, value);
        }

        public override string Description => $"Проверка места на дисках на {Server}";
        
        public override async Task ExecuteRoutineTest()
        {
            try
            {
                using (var client = new AgentServiceClient(
                     new NetTcpBinding(),
                     new EndpointAddress(@"net.tcp://" + Server + @":9002/DailyHelperAgent")))
                {
                    DrivesFreeSpace = client.GetDrivesFreeSpace();
                }

                var results = DrivesFreeSpace.Select(df => $"{df.Name} - {Helpers.ShareDetector.GetHumanReadableFreeSpace(df.FreeSpace)}");

                Result = results.Aggregate((a, b) => a + $"\n{b}");
                Success = true;
            }
            catch (CommunicationObjectFaultedException)
            {
                Result = "Не удалась связаться с Агентом";
                Success = false;
            }
            catch (Exception e)
            {
                Result = $"Ошибка: {e.GetBaseException().Message}";
                Success = false;
            }

        }
    }
}
