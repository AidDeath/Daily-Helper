using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class FolderLastChangedRoutine : RoutineBase
    {
        private int _maxMinSinceChanged = 10;
        /// <summary>
        /// Desired maximum tipe pass from last folder change
        /// </summary>
        public int MaxMinSinceChanged
        {
            get => _maxMinSinceChanged;
            set => SetProperty(ref _maxMinSinceChanged, value);
        }

        private string _path = "";
        /// <summary>
        /// Path to directory
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage ="Необходимо указать путь к директории")]
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }


        public override string Description => $"Изменения в каталоге: {Path} за последние {MaxMinSinceChanged} минут";

        public override async Task ExecuteRoutineTest()
        {
            try
            {

                var dirInfo = new DirectoryInfo(Path);

                if (!dirInfo.Exists) throw new Exception("Указанная папка недоступна!");

                var lastChangedDateTime = new DirectoryInfo(Path).LastWriteTime;

                var timePassed = DateTime.Now - lastChangedDateTime;

                bool isLessThanMaxMinutesPass = ((int)timePassed.TotalMinutes) < MaxMinSinceChanged;

                Success = isLessThanMaxMinutesPass;
                Result = $"Изменения в папке: {(int)timePassed.TotalMinutes} минут назад";
            }
            catch (Exception ex)
            {
                Success = false;
                Result = $"Ошибка при проверке папки: {ex.GetBaseException().Message}";

            }


            

        }
    }
}
