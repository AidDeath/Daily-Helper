using Daily_Helper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Daily_Helper.Models
{
    /// <summary>
    /// Checking free disk space at selected path (May be network pass)
    /// </summary>
    public class FileShareRoutine : RoutineBase
    {
        private string _server;
        public string Server
        {
            get => _server;
            set => SetProperty(ref _server, value);
        }

        private IEnumerable<Share> _allShares;

        private ObservableCollection<Share>? _watchingShares;
        /// <summary>
        /// File shares that would be checking in routine
        /// </summary>
        public ObservableCollection<Share>? WatchedShares
        {
            get => _watchingShares;
            set => SetProperty(ref _watchingShares, value);
        }

        public override string Description => $"Проверка файловых шар на {Server}";


        //public FileShareRoutine(string server)
        //{
        //    //Description = $"Проверка файловых шар на {server}";

        //    Server = server;
        //    try
        //    {
        //        _allShares = ShareDetector
        //        .GetAllShares(Server)
        //        .Where(share => share.IsFileSystem);
        //    }
        //    catch (Exception e)
        //    {
        //        IsActivated = false;
        //        Result = $"Ошибка - {e.GetBaseException().Message}\nПроверка отключена";

        //    }
        //}

        public override async Task ExecuteRoutineTest()
        {
            var results = new List<string>();
            try
            {
                foreach (var share in WatchedShares)
                {
                    await Task.Run(() => share.RefreshFreeSpace()); 
                    results.Add($"{share.NetName} - {ShareDetector.GetHumanReadableFreeSpace(share.FreeSpace)} свободно");
                    
                }

                Success = true;
                Result = results.Aggregate((a, b) => a + $"\n{b}");
            }
            catch (Exception)
            {
                Success = false;
                //Result = $"Ошибка: {e.Message}";
            }



        }
    }
}
