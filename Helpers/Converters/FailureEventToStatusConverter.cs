using Daily_Helper.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Daily_Helper.Helpers.Converters
{
    public class FailureEventToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as FailureEvent is not null)
            {
                var a = value as FailureEvent;
                return $"{(a.IsStillActive? "Не исправлена" : "Исправлена")}, {(a.RoutineIdentifer.IsCurrentlyInList? "проверяется": "не проверяется")}";
            }

            return string.Empty;
                
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
