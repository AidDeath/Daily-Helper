using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper.Models
{
    /// <summary>
    /// Таблица адресатов почты
    /// </summary>
    public class Email : ObservableObject
    {
        //Идентификатор
        public int Id { get; set; }
        
        //Полное имя получателя почты
        public string FullName { get; set; }
        
        //Почтовый адрес
        public string EmailAddress { get; set; }


    }
}
