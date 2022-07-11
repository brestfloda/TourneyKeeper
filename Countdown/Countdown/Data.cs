using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countdown
{
    public class Data
    {
        public Data()
        {
            RoundEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.AddHours(4).Hour, 0, 0);
            TimeLeft = RoundEnd.Subtract(DateTime.Now);
        }

        public string Tournamentname { get; set; }
        public DateTime RoundEnd { get; set; }
        public TimeSpan TimeLeft { get; set; }
    }
}
