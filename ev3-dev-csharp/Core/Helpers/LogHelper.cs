using System;
using log4net;

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
					log.Info($"[  OK  ]  {string.Format(format, args)}");
					break;
				case Helpers.Status.KO:
					log.Info($"[--KO--]  {string.Format(format, args)}");
					break;
			}
			
		}
	}
}
