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

namespace HeadControl
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
				Logger.Clear();

				var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
				var version = fvi.FileVersion;
				Logger.Info($"###### EV3 Head Control ({version}) ######");

				var soundManager = ev3Services.GetService<ISoundManager>();
				var headControl = new HeadControl(Inputs.Input4, Outputs.OutputB, Logger);
				soundManager.PlaySound("ready");
				headControl.Calibrate();
				Console.Clear();
					
				while (true)
				{
					Thread.Sleep(100);
					if (Console.KeyAvailable)
					{
						var key = Console.ReadKey();
						if (key.Key == ConsoleKey.Escape)
							break;
					}

					headControl.Update();
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
