using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Sensors;
using EV3.Dev.Csharp.Services;
using EV3.Dev.Csharp.Services.Sound;
using log4net;
using log4net.Repository.Hierarchy;

namespace MotorControl
{
	public class Program
	{
		private static ILog Logger { get; set; }

		public static void Main(string[] args)
		{
			try
			{
				var ev3Services = Ev3Services.Instance;
				Logger = ev3Services.GetService<ILog>();
				var soundManager = ev3Services.GetService<ISoundManager>();
				Logger.Clear();

				var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
				var version = fvi.FileVersion;
				Logger.Info($"###### EV3 Motor Control ({version}) ######");
				var shell = new Shell(Logger);
				soundManager.PlaySound("ready");

				while (true)
				{
					if (Console.KeyAvailable)
					{
						var key = Console.ReadKey();
						if (key.Key == ConsoleKey.Escape)
							break;
					}

					var cmd = Console.ReadLine();
					var result = shell.Eval(cmd);
					if(result != null)
						Logger.Status(Status.OK, $"{result}");
				}
			}
			catch (Exception ex)
			{
				if (Logger != null)
				{
					Logger.Status(Status.KO, "Unexpected error");
					Console.ReadKey();

					Logger.Error("Stack trace", ex);
					Console.ReadKey();
				}
			}
		}
	}
}
