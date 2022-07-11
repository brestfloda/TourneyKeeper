using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourneyKeeper.Common
{
    public static class Currency
    {
        public static List<string> Currencies = new List<string>()
        {
"EUR",
"DKK",
"USD",
        };

        public static string FormatCurrency(decimal amount, string currencyCode)
        {
            try
            {
                var culture = (from c in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                               let r = new RegionInfo(c.LCID)
                               where r != null
                               && r.ISOCurrencySymbol.ToUpper() == currencyCode.ToUpper()
                               select c).FirstOrDefault();
                return string.Format(culture, "{0:C}", amount);
            }
            catch
            {
                return amount.ToString("0.00");
            }
        }
    }
}
