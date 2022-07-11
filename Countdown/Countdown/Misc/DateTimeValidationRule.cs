using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Countdown.Misc
{
    public class DateTimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value.ToString();
            if(string.IsNullOrEmpty(input))
            {
                return new ValidationResult(true, null);
            }
            DateTime dateTime;

            if (DateTime.TryParseExact(input, "d-M-yyyy H:m", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Format: dd-mm-yyyy hh:mm");
            }
        }
    }
}
