using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Countdown.Misc
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            DateTime val = (DateTime)value;

            return val.ToString("d-M-yyyy HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value.ToString();
            if (string.IsNullOrEmpty(input))
            {
                return new DateTime?();
            }
            DateTime dateTime;

            if (DateTime.TryParseExact(input, "d-M-yyyy H:m", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }
            else
            {
                throw new ApplicationException("Format: dd-mm-yyyy hh:mm");
            }
        }
    }
}
