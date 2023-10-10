using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class ServiceStateRoutine : RoutineBase
    {

        public ServiceStateRoutine()
        {
        }

        private string _server;
        public string Server
        {
            get => _server;
            set => SetProperty(ref _server, value);
        }

        private IEnumerable<ServiceInfo> _allServices;

        private ObservableCollection<ServiceInfo> _watchedServices;
        /// <summary>
        /// Services that are selected to be watched
        /// </summary>
        public ObservableCollection<ServiceInfo> WatchedServices
        {
            get => _watchedServices;
            set => SetProperty(ref _watchedServices, value);
        }

        public override string Description => $"Состояние служб на {Server}";

        public override async Task ExecuteRoutineTest()
        {

            var results = new List<string>();
            try
            {
                foreach (var service in WatchedServices)
                {
                    try
                    {
                        var sc = new ServiceController(service.Name, service.MachineName);
                        service.Status = sc.Status.Equals(ServiceControllerStatus.Running) ? "Работает" : "Остановлена";
                    }
                    catch (InvalidOperationException ex)
                    {
                        var baseEx = ex.GetBaseException();
                        service.Status = ex.GetBaseException().Message;
                        service.IsSelected = false;
                    }

                    service.IsSuccess = true;
                    results.Add($"{service.Name} - {service.Status}");

                }

                //success if all services are running
                Success = WatchedServices.All(s => s.IsSuccess && s.Status == "Работает");
                Result = results.Aggregate((a, b) => a + $"\n{b}");
            }
            catch (Exception e)
            {
                Success = false;
                Result = $"Ошибка: {e.Message}";
            }
        }
    }
}
