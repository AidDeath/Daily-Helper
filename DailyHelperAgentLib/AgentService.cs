using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DailyHelperAgentLib
{
    public class AgentService : IAgentService
    {
        public List<string> GetProcessList()
        {
            return Process.GetProcesses().OrderBy(p => p.ProcessName).Select(p => p.ProcessName).Distinct().ToList();
        }


        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint SearchPath(string lpPath,
            string lpFileName,
            string lpExtension,
            int nBufferLength,
            [MarshalAs ( UnmanagedType.LPTStr )]
            StringBuilder lpBuffer,
            out IntPtr lpFilePart);

        const int MAX_PATH = 260;

        static string SearchProcessPath(string processName)
        {
            StringBuilder sb = new StringBuilder(MAX_PATH);
            IntPtr discard;
            var nn = SearchPath(null, $"{processName}.exe", null, sb.Capacity, sb, out discard);
            if (nn == 0)
            {
               return string.Empty;
            }
            else
                return sb.ToString();
        }


        public ProcessState GetProcessState(string processName)
        {
            var proceses = Process.GetProcessesByName(processName);

            return new ProcessState()
            {
                Name = processName,
                IsFound = proceses.FirstOrDefault() != null,
                IsResponding = proceses != null && proceses.All(proc => proc.Responding),
                FullProcessPath = SearchProcessPath(processName)
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

        public void RunProcess(string executablePath)
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = executablePath;
                process.Start();
            }
            catch (Exception)
            {
                return;
            }

        }
    }
}
