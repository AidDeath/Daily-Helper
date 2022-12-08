using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    /// <summary>
    /// Object that represents routine task
    /// </summary>
    public abstract class RoutineBase : ValidatingObservableObject //ObservableObject
    {
        public RoutineBase()
        {
            IsActivated = true;
        }


        private bool _isActivated;

        public bool IsActivated
        {
            get => _isActivated;
            set => SetProperty(ref _isActivated, value);
        }

        private bool _success;
        /// <summary>
        /// Result of routine test
        /// </summary>
        public bool Success 
        {
            get => _success;
            set
            {
                SetProperty(ref _success, value);
                if (value) LastSucceeded = DateTime.Now;
            }
        }

        //private string _description = "unnamed routine test";
        ///// <summary>
        ///// Description of routine
        ///// </summary>
        //public string Description 
        //{ 
        //    get => _description; 
        //    set => SetProperty(ref _description, value); 
        //}

        [JsonIgnore]
        public abstract string Description { get; }

        private string _result = "Не выполнялось";
        /// <summary>
        /// Result of routine test execution
        /// </summary>
        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        private DateTime _lastExecutted;
        /// <summary>
        /// Last time when current test was executed
        /// </summary>
        public DateTime LastExecutted
        {
            get => _lastExecutted;
            set => SetProperty(ref _lastExecutted, value);
        }

        private DateTime _lastSucceded;

        public DateTime LastSucceeded
        {
            get => _lastSucceded;
            set => SetProperty(ref _lastSucceded, value);
        }


        /// <summary>
        /// Do the test and refresh info;
        /// </summary>
        /// <returns></returns>
        public abstract Task ExecuteRoutineTest();

        public SerializedRoutine GetSerialized()
        {
            var currentType = GetType();

            return new SerializedRoutine()
            {
                Type = currentType,
                JsonString = JsonSerializer.Serialize(this, currentType, new JsonSerializerOptions() { AllowTrailingCommas = true, WriteIndented = true }),
            };
        }

    }
}
