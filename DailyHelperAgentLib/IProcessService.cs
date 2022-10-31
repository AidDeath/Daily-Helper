using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DailyHelperAgentLib
{
    [ServiceContract]
    public interface IProcessService
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

    }

    [DataContract]
    public class ProcessState
    {
        private string name;
        private bool isFound;
        private bool isResponding;

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
    }

}
