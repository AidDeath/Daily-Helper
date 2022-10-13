using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class ConnPortRoutine : RoutineBase
    {
        private string? _hostname;
        [Required(ErrorMessage ="Укажите имя хоста")]
        public string? Hostname
        {
            get => _hostname;
            set => SetProperty(ref _hostname, value);
        }

        private int _port;
        [Range(1,99999, ErrorMessage = "Неверный порт")]
        [Required(ErrorMessage = "Укажите порт")]
        public int Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        public override string Description => $"Проверка подключения к {Hostname}:{Port} ";
        
        public ConnPortRoutine(string hostname, int port)
        {
            Hostname = hostname;
            Port = port;
            //Description = $"Проверка подключения к {Hostname}:{Port} ";
        }

        public override async Task ExecuteRoutineTest()
        {        
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //TODO: Позже отлавливать валидацией
                if (Hostname is null || Hostname.Length < 1) throw new Exception("Не указан хост");
                await socket.ConnectAsync(Hostname, Port);

                Success = true;
                Result = $"Подключение успешно";
            }
            catch (SocketException e)
            {
                Success = false;
                if (e.ErrorCode == 10061)
                    Result = $"Сервис на порту не отвечает";
                else

                    if (e.ErrorCode == 10060) Result = $"Сервер не отвечает";
                else
                    Result = $"Ошибка {e.GetBaseException().Message}";
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
