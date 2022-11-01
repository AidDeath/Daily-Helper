using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DailyHelperAgentLib
{
    public class AgentService : IAgentService
    {
        public List<string> GetProcessList()
        {
            return Process.GetProcesses().OrderBy(p => p.ProcessName).Select(p => p.ProcessName).Distinct().ToList();
        }

        public ProcessState GetProcessState(string processName)
        {
            var proceses = Process.GetProcessesByName(processName);
  
            return new ProcessState()
            {
                Name = processName,
                IsFound = proceses.FirstOrDefault() != null,
                IsResponding = proceses != null && proceses.All(proc => proc.Responding)
            };
        }

        public List<DriveFreeSpace> GetDrivesFreeSpace()
        {
            var driveInfos = DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Fixed).ToArray();
            var driveFreeSpaces = new List<DriveFreeSpace>();

            foreach (var drive in driveInfos)
            {
                driveFreeSpaces.Add(new DriveFreeSpace() { Name = drive.Name, FreeSpace = drive.AvailableFreeSpace });
            }

            return driveFreeSpaces;
        }

    }
}
