using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Helpers.Enums
{
    public enum RoutineTypes
    {
        ConnectToPort,
        Ping,
        FileShare,
        ServiceState,
        ProcessState,
        DriveFreeSpace
    }

    /// <summary>
    /// Describing of a routine tasks
    /// </summary>
    public static class RoutinesDescriptions
    {
        public static Dictionary<RoutineTypes, string> Descriptions = new()
        {
            { RoutineTypes.ConnectToPort, "Проверка подключения на порт" },
            { RoutineTypes.FileShare, "Проверка файловых шар" },
            { RoutineTypes.Ping, "Проверка доступности хоста (ping)" },
            { RoutineTypes.ServiceState, "Проверка состояния службы" },
            { RoutineTypes.ProcessState, "Проверка состояния процесса" },
            { RoutineTypes.DriveFreeSpace, "Проверка места на дисках" }
        };
    }

}
