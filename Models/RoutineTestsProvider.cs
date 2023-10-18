using Daily_Helper.Helpers;
using Daily_Helper.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    /// <summary>
    /// Class for value providing from background services to ViewModels or other
    /// </summary>
    public class RoutineTestsProvider
    {
        public RoutineTestsProvider()
        {
            //SelectedRoutines.CollectionChanged += RoutinesModified;
        }
        public ObservableCollection<RoutineBase> Routines { get; set; } = new();

        private void RoutinesModified(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            throw new NotImplementedException();
        }
    }
}
