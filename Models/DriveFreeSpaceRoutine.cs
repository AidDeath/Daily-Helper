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

        private int _desiredFreeSpaceGb;

        public int DesiredFreeSpaceGb
        {
            get => _desiredFreeSpaceGb;
            set => SetProperty(ref _desiredFreeSpaceGb, value);
        }

        public DriveFreeSpaceRoutine(string server, int desiredFreeSpaceGb)
        {
            Server = server;
            DesiredFreeSpaceGb = desiredFreeSpaceGb;
        }

        private DriveFreeSpace[] drivesFreeSpace;
        public DriveFreeSpace[] DrivesFreeSpace
        {
            get => drivesFreeSpace;
            set => SetProperty(ref drivesFreeSpace, value);
        }

        public override string Description => $"Проверка места на дисках на {Server} \nПорог: {DesiredFreeSpaceGb} Gb";
        
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

                var results = DrivesFreeSpace.Select(df => $"{df.Name} - {Helpers.ShareDetector.GetHumanReadableFreeSpace(df.FreeSpace)}  свободно");

                Result = results.Aggregate((a, b) => a + $"\n{b}");

                //If all of disks are above desired free space - it's ok
                Success = DrivesFreeSpace.All(a => a.FreeSpace > DesiredFreeSpaceGb * 1024d * 1024d * 1024d);
            }
            catch (CommunicationObjectFaultedException)
            {
                Result = "Не удалось связаться с Агентом";
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
