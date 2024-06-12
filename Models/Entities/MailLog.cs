using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    /// <summary>
    /// Таблица для хранения протокола отправки почтовых уведомлений
    /// </summary>
    public class MailLog
    {
        // Идентификатор записи
        public int Id { get; set; }
        [ForeignKey("FailureEvent")]

        //Идентификатор записи журнала событий от ошибке
        public int FailureEventId { get; set; }
        [ForeignKey("Email")]

        //Идентификатор получателя письма
        public int EmailId { get; set; }

        //Дата и время отправки почтового уведомления
        public DateTime SendAt { get; set; }
        
        //Тем письма 
        public string Subject { get; set; }

        //Статус отправки письма (успешно  / неуспешно)
        public string SendResult { get; set; }

        public FailureEvent FailureEvent { get; set; }
        public Email Email { get; set; }

    }
}
