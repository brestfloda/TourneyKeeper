using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace TourneyKeeper.Test
{
    [TestClass]
    public class LoadTest
    {
        [TestMethod]
        public void CreateTestUsersAndTeams()
        {
            var adr = new string[]
            {
                //"http://localhost:20933//Team/TKTeamLeaderboard.aspx?Id=257",
                "http://localhost:20933//Team/TKIndividualLeaderboard.aspx?id=257",
                //"http://localhost:20933//Team/TKTeampairings.aspx?id=257",
                //"http://localhost:20933//Team/TKIndividualpairings.aspx?id=257",
                //"http://localhost:20933//"
                //"http://eb.dk/"
            };

            var lockMe = new Object();
            var list = new List<Tuple<string, TimeSpan>>();
            Random rand = new Random((int)DateTime.Now.Ticks);
            ParallelOptions po = new ParallelOptions();

            Stopwatch totalStopWatch = new Stopwatch();
            totalStopWatch.Start();
            Parallel.For(0, 1000, po, new Action<int>(i =>
            {
                Thread.Sleep(rand.Next(1000));

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                WebClient client = new WebClient();
                string a = adr[rand.Next(adr.Length - 1)];
                client.DownloadData(a);
                stopWatch.Stop();

                lock (lockMe)
                {
                    list.Add(new Tuple<string, TimeSpan>(a, stopWatch.Elapsed));
                }
            }));
            totalStopWatch.Stop();

            var sort = list.OrderBy(x => x.Item2);

            Trace.Listeners.Add(new ConsoleTraceListener());
            Trace.WriteLine($"Time elapsed: {totalStopWatch.Elapsed}");
            Trace.WriteLine($"Min: {sort.Select(x => x.Item2).Min()}");
            Trace.WriteLine($"Max: {sort.Select(x => x.Item2).Max()}");
            Trace.WriteLine($"Average (ms): {sort.Select(x => x.Item2.TotalMilliseconds).Average()}");

            var min = list.GroupBy(y => y.Item1).Select(w => new { Page = w.Key, Min = w.Min() });
            foreach (var m1 in min)
            {
                Trace.WriteLine($"Min: {m1.Page}: {m1.Min.Item2}");
            }
            var max = list.GroupBy(y => y.Item1).Select(w => new { Page = w.Key, Max = w.Max() });
            foreach (var m2 in max)
            {
                Trace.WriteLine($"Max: {m2.Page}: {m2.Max.Item2}");
            }
            var ave = list.GroupBy(y => y.Item1).Select(w => new { Page = w.Key, Average = w.Average(e => e.Item2.TotalMilliseconds) });
            foreach (var a in ave)
            {
                Trace.WriteLine($"Average (ms): {a.Page}: {a.Average}");
            }

            foreach (var b in list)
            {
                Trace.WriteLine($"{b.Item2.TotalMilliseconds}");
            }
        }
    }
}
