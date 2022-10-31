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
    }
}
