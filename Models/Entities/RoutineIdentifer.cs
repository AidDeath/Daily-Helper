using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    /// <summary>
    /// Таблица хранящая записи, идентифицирующие задачи
    /// </summary>
    public class RoutineIdentifer
    {
        //Идентификатор записи
        [Key]
        public string RoutineId { get; set; }

        // признак нахождения текущей задачи в списке проверки
        public bool IsCurrentlyInList { get; set; }

        //Описание задачи проверки
        public string Description { get; set; }

        public ICollection<MailReciever> MailRecievers { get; set; }
        public ICollection<FailureEvent> FailureEvents { get; set; }

    }
}
