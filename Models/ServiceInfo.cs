using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class ServiceInfo : ObservableObject
    {
        private string _Name;
        /// <summary>
        /// Service short name
        /// </summary>
        public string Name 
        { 
            get => _Name;
            set => SetProperty(ref _Name, value); 
        }

        private string _description;
        /// <summary>
        /// Description of a service
        /// </summary>
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _status;
        /// <summary>
        /// Status message
        /// </summary>
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private bool _isSelected;
        /// <summary>
        /// Is this share selected in list
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public bool IsSuccess;

        public string MachineName { get; set; } = string.Empty;

        public static IEnumerable<ServiceInfo> GetAllServices(string serverName)
        {
            var servicesControllers = ServiceController.GetServices(serverName);
            var services = new List<ServiceInfo>();

            foreach (var serviceController in servicesControllers)
            {
                services.Add(new ServiceInfo()
                {
                    Name = serviceController.ServiceName,
                    Description = serviceController.DisplayName,
                    Status = serviceController.Status.Equals(ServiceControllerStatus.Running) ? "Работает" : "Остановлен",
                    MachineName = serviceController.MachineName
                }) ;
            }

            return services;
        }
    }
}
