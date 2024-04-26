using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class MailReciever
    {
        public int Id { get; set; }

        [ForeignKey("RoutineIdentifer")]
        public string RoutineId { get; set; }

        [ForeignKey("Email")]
        public int EmailAddressId { get; set; }

        public RoutineIdentifer RoutineIdentifer { get; set; }

        public Email Email { get; set; }


    }
}
