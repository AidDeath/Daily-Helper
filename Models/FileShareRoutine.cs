using Daily_Helper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Daily_Helper.Models
{
    /// <summary>
    /// Checking free disk space at selected path (May be network pass)
    /// </summary>
    internal class FileShareRoutine : RoutineBase
    {
        private string _server;
        private IEnumerable<Share> _allShares;

        private ObservableCollection<Share> _watchingShares;
        /// <summary>
        /// File shares that would be checking in routine
        /// </summary>
        public ObservableCollection<Share> WatchedShares
        {
            get => _watchingShares;
            set => SetProperty(ref _watchingShares, value);
        }

        public FileShareRoutine(string ServerPath)
        {
            Description = $"Проверка файловых шар на {ServerPath}";

            _server = ServerPath;
            try
            {
                _allShares = ShareDetector
                .GetAllShares(_server)
                .Where(share => share.IsFileSystem);
            }
            catch (Exception e)
            {
                IsActivated = false;
                Result = $"Ошибка - {e.GetBaseException().Message}\nПроверка отключена";

            }
            
            if (_allShares is not null)
                WatchedShares = new (_allShares);

        }

        public override async Task ExecuteRoutineTest()
        {
            var results = new List<string>();
            try
            {
                foreach (var share in WatchedShares)
                {
                    share.RefreshFreeSpace();
                    results.Add($"{share.NetName} - {share.FreeSpace} свободно");
                    
                }

                Success = true;
                Result = results.Aggregate((a, b) => a + $"\n{b}");
            }
            catch (Exception еx)
            {
                Success = false;
                //Result = $"Ошибка: {ex.Message}";
            }



        }
    }
}
