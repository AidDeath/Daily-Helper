using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    /// <summary>
    /// Gets IP address of server and sends a ICMP request
    /// </summary>
    /// <param name="Hostname"></param>
    public class PingRoutine : RoutineBase
    {

        Ping pingSender = new Ping();
        PingOptions pingOptions = new PingOptions() {  DontFragment = true, Ttl = 128};

        private string _hostname;
        public string Hostname
        {
            get => _hostname;
            set => SetProperty(ref _hostname, value);
        }

        public PingRoutine(string hostname)
        {
            Hostname = hostname;
            //Description = $"Команда ping на сервер {_hostname}";
        }

        public override string Description => $"Команда ping на сервер {Hostname}";

        public override async Task ExecuteRoutineTest()
        {
            PingReply pingReply;
            try
            {
                pingReply = await pingSender.SendPingAsync(_hostname);
                if (pingReply == null) throw new Exception("Unknown error");
            }
            catch (Exception ex)
            {
                Success = false;
                Result = $"Ошибка команды ping - {ex.GetBaseException().Message}";
                return;
            }

            if (pingReply.Status == IPStatus.Success)
            {
                Success = true;
                Result = $"Сервер доступен, время отклика: {pingReply.RoundtripTime}мс.";
            }
            else
            {
                Success=false;
                Result = "Ошибка. Сервер не отвечает";
            } 
            
        }
    }
}
