using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class FailureEvent : ObservableObject
    {
        public int Id { get; set; }

        [ForeignKey("RoutineIdentifer")]
        public string RoutineId { get; set; }
        public DateTime Occured { get; set; }
        public string FailureDescription { get; set; } = "Не указано";
        public string ExceptionMessage { get; set; } = "Не указано";
        public bool IsStillActive { get; set; } = true;

        public RoutineIdentifer RoutineIdentifer { get; set; }



        public override string ToString()
        {
            return $"Routine {RoutineId}, Occured {Occured}, Error:{ExceptionMessage}, Active:{(IsStillActive ? "Yep" : "Nah")}";
        }
    }
}
