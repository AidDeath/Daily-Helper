using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class DiskFreeSpaceRoutine : RoutineBase
    {
        private string _server;
        public string Server
        {
            get => _server;
            set => SetProperty(ref _server, value);
        }

        public override string Description => $"Проверка места на дисках на {Server}";
        public override Task ExecuteRoutineTest()
        {
            throw new NotImplementedException();
        }
    }
}
