using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    public class MailLog
    {
        public int Id { get; set; }
        [ForeignKey("FailureEvent")]
        public int FailureEventId { get; set; }
        [ForeignKey("Email")]
        public int EmailId { get; set; }
        public DateTime SendAt { get; set; }
        public string Subject { get; set; }

        public FailureEvent FailureEvent { get; set; }
        public Email Email { get; set; }

    }
}
