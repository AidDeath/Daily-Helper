using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DailyHelperAgentLib
{
    public class ProcessService : IProcessService
    {
        public List<string> GetProcessList()
        {
            return Process.GetProcesses().OrderBy(p => p.ProcessName).Select(p => p.ProcessName).ToList();
        }

        public ProcessState GetProcessState(string processName)
        {
            var process = Process.GetProcessesByName(processName).FirstOrDefault(p => p.ProcessName == processName);

            return new ProcessState()
            {
                Name = processName,
                IsFound = process != null,
                IsResponding = process != null && process.Responding
            };



        }
    }
}
