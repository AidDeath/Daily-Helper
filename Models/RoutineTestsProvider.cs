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
        public ObservableCollection<RoutineBase> Routines { get; set; } = new();
    }
}
