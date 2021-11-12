using log4net;
using System;

namespace EV3.Dev.Csharp.Core.Helpers
{
    public static class LogHelper
    {
        public static void Clear(this ILog log)
        {
            Console.Clear();
        }

        public static void Status(this ILog log, Status status, string format, params object[] args)
        {
            switch (status)
            {
                case Helpers.Status.OK:
                    log.Debug($"[  OK  ]  {string.Format(format, args)}");
                    break;
                case Helpers.Status.KO:
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        log.Debug($"[  KO  ]  {string.Format(format, args)}");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }

            }
        }
    }
}
