using System;
using System.Diagnostics;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Common.Managers
{
    public static class LogManager
    {
        public static void LogEvent(string message)
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);
            var header = $"{sf.GetMethod().ReflectedType.FullName}.{sf.GetMethod().Name}";

            Log("Event", header, message);
        }

        public static void LogEvent(string header, string message)
        {
            Log("Event", header, message);
        }

        public static void LogError(string message)
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);
            var header = $"{sf.GetMethod().ReflectedType.FullName}.{sf.GetMethod().Name}";

            Log("Error", header, message);
        }

        private static void Log(string logtype, string header, string message)
        {
            try
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var log = new Log
                    {
                        LogType = logtype,
                        Header = header,
                        Message = message,
                        DateTime = DateTime.Now
                    };

                    context.Log.Add(log);
                    context.SaveChanges();
                }
            }
            catch
            {
                //bare ærgerligt...
            }
        }
    }
}
