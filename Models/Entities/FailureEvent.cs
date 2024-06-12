using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    /// <summary>
    /// Таблица лога событий отказов, возникших при проверке задач
    /// </summary>
    public class FailureEvent : ObservableObject
    {
        //Идентификатор
        public int Id { get; set; }

        //Идентификатор записи идентификатора события
        [ForeignKey("RoutineIdentifer")]
        public string RoutineId { get; set; }

        //Дата и время обнаружения сбоя
        public DateTime Occured { get; set; }

        //Описание обнаруженного сбоя
        public string FailureDescription { get; set; } = "Не указано";

        //Описание ошибки, возникшей при проверке
        public string ExceptionMessage { get; set; } = "Не указано";
        
        //Признак активности ошибки (Исправлена ли она)
        public bool IsStillActive { get; set; } = true;

        public RoutineIdentifer RoutineIdentifer { get; set; }

        public string MailReceiversText
        {
            get => RoutineIdentifer?.MailRecievers?.Count > 0 ? $"{RoutineIdentifer.MailRecievers.Select(mr => mr.Email.FullName).Aggregate((a, b) => a + "\n" + b)}" : "Не назначено";
        }
        public override string ToString()
        {
            return $"Routine {RoutineId}, Occured {Occured}, Error:{ExceptionMessage}, Active:{(IsStillActive ? "Yep" : "Nah")}";
        }
    }
}
