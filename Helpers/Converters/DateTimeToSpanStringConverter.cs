using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Daily_Helper.Helpers.Converters
{
    public enum SpanTextType
    {
        Exact,
        TextDescrirtion
    }
    /// <summary>
    /// Converter takes DateTime and returns string with text description of time passed to current
    /// </summary>
    public class DateTimeToSpanStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || (DateTime)value == default(DateTime)) return "Никогда";
            var timespan = DateTime.Now - (DateTime)value;

            if (timespan < TimeSpan.FromMinutes(1)) 
                return 
                    (parameter is not SpanTextType || (SpanTextType)parameter == SpanTextType.TextDescrirtion) 
                    ? "Только что" 
                    : $"{(DateTime)value :dd.MM HH:mm:ss}";

            return $"{timespan.TotalMinutes} мин. назад";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
