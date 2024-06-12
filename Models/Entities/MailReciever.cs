using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    /// <summary>
    /// Таблица сопоставления задач с получателями почты
    /// </summary>
    public class MailReciever
    {
        //Идентификатор
        public int Id { get; set; }

        // Идентификатор задачи
        [ForeignKey("RoutineIdentifer")]
        public string RoutineId { get; set; }

        //Идентификатор почтового адреса
        [ForeignKey("Email")]
        public int EmailAddressId { get; set; }

        public RoutineIdentifer RoutineIdentifer { get; set; }

        public Email Email { get; set; }


    }
}
