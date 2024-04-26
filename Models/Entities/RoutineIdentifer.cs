using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class RoutineIdentifer
    {
        [Key]
        public string RoutineId { get; set; }
        public bool IsCurrentlyInList { get; set; }
        public string Description { get; set; }

        public ICollection<MailReciever> MailRecievers { get; set; }
        public ICollection<FailureEvent> FailureEvents { get; set; }

    }
}
