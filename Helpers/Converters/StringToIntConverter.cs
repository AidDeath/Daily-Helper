using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Daily_Helper.Helpers.Converters
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || (value is string && string.IsNullOrWhiteSpace((string)value))) return 0;

            string val = (string)value;

            Regex regex = new Regex("^[0-9]+$");
            if (regex.IsMatch(val) && val.Length < 8) return int.Parse(val);

            string correctedValue = string.Empty;
            foreach (var symbol in val)
            {
                if (regex.IsMatch(symbol.ToString())) correctedValue += symbol;
            }
            return correctedValue;
        }
    }
}
