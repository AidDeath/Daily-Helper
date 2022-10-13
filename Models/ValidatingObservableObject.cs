using System;
using System.Collections.Concurrent;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public abstract class ValidatingObservableObject : ObservableObject, INotifyDataErrorInfo
    {
        public ValidatingObservableObject() : base()
        {
            ValidateAll();
        }


        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName != null)
            {
                //ValidateProperty(propertyName, this);
                ValidateAll();
            }
        }

        /// <summary>
        /// object for locking concurrent computing
        /// </summary>
        private readonly object _lock = new();

        /// <summary>
        /// Dictionary for storing all validating errors of current object
        /// </summary>
        private ConcurrentDictionary<string, List<string>> _errors = new();


        public bool HasErrors
        {
            get
            {
                lock (_lock)
                {
                    return _errors.Any(propErrors => propErrors.Value != null && propErrors.Value.Count > 0);
                }
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                lock (_lock)
                {
                    if (_errors.ContainsKey(propertyName) && (_errors[propertyName] != null) && _errors[propertyName].Count > 0)
                    {
                        return _errors[propertyName].ToList();
                    }
                    else
                    {
                        return null;
                    }
                }

            }

            lock (_lock)
            {
                return _errors.SelectMany(err => err.Value.ToList());
            }
        }

        protected void ValidateAll()
        {
            var validationContext = new ValidationContext(this, null, null);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(this, validationContext, validationResults, true);

            foreach (var kv in _errors.ToList().Where(kv => validationResults.All(r => r.MemberNames.All(m => m != kv.Key))))
            {
                _errors.TryRemove(kv.Key, out var outList);
                OnErrorsChanged(kv.Key);
            }


            var goupedResults = from r in validationResults
                                from m in r.MemberNames
                                group r by m into g
                                select g;

            foreach (var entry in goupedResults)
            {
                var messages = entry.Select(r => r.ErrorMessage).ToList();
                if (_errors != null && _errors.ContainsKey(entry.Key))
                {
                    _errors?.TryRemove(entry.Key, out var outList);
                }

                _errors?.TryAdd(entry.Key, messages);
                OnErrorsChanged(entry.Key);

            }
        }

        //protected void ValidateProperty<T>(string propertyName, T value)
        //{
        //    var validationResults = new List<ValidationResult>();
        //    if (validationResults.Any())
        //    {
        //        _errors[propertyName] = validationResults.Select(c => c.ErrorMessage).ToList();  
        //    }
        //    else
        //    {
        //        lock (_lock)
        //        {
        //            _errors.TryRemove(propertyName, out var outList);
        //        }
        //    }

        //    OnErrorsChanged();
        //}
    }
}
