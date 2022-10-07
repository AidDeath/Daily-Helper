using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class ConnPortRoutine : RoutineBase
    {
        private string _hostname;
        private int _port;

        /// <summary>
        /// Socket object for connection testing
        /// </summary>
        

        public ConnPortRoutine(string Hostname, int Port)
        {
            _hostname = Hostname;
            _port = Port;
            Description = $"Проверка подключения к {Hostname}:{Port} ";

            
        }
        public override async Task ExecuteRoutineTest()
        {        
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                await socket.ConnectAsync(_hostname, _port);

                Success = true;
                Result = $"Подключение успешно";
                

            }
            catch (SocketException e)
            {
                Success = false;
                if (e.ErrorCode == 10061)
                    Result = $"Порт закрыт";
                else

                    if (e.ErrorCode == 10060) Result = $"Сервер не отвечает";
                else
                    Result = $"Ошибка ${e.GetBaseException().Message}";
                throw;
            }
            catch (Exception e)
            {
                Success = false;
                Result = $"Ошибка ${e.GetBaseException().Message}";
            }
            finally
            {
                if (socket.Connected) socket.Close();
            }

            
        }
    }
}
