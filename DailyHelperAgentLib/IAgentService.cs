using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DailyHelperAgentLib
{
    [ServiceContract]
    public interface IAgentService
    {
        /// <summary>
        /// Get process list from machine running this service
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<string> GetProcessList();

        /// <summary>
        /// Get ProcessState object for process on machine running service by name
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        [OperationContract]
        ProcessState GetProcessState(string processName);

        [OperationContract]
        List<DriveFreeSpace> GetDrivesFreeSpace();

        [OperationContract]
        void RunProcess(string executablePath);

    }

    [DataContract]
    public class DriveFreeSpace
    {
        private string name;
        private long freeSpace;

        [DataMember]
        public string Name
        {
            get => name;
            set => name = value;
        }

        [DataMember]
        public long FreeSpace
        {
            get => freeSpace;
            set => freeSpace = value;
        }
    }

    [DataContract]
    public class ProcessState
    {
        private string name;
        private bool isFound;
        private bool isResponding;
        private string fullProcessPath;

        [DataMember]
        public string Name
        {
            get => name;
            set => name = value;
        }

        [DataMember]
        public bool IsFound
        {
            get => isFound;
            set => isFound = value;
        }

        [DataMember]
        public bool IsResponding
        {
            get => isResponding;
            set => isResponding = value;
        }

        [DataMember]
        public string FullProcessPath
        {
            get => fullProcessPath;
            set => fullProcessPath = value;
        }
    }
}
