using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Sensors;
using EV3.Dev.Csharp.Services;
using EV3.Dev.Csharp.Services.Sound;
using log4net;

namespace IrRemote
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
				Logger.Info($"###### EV3 Infrared Remote ({version}) ######");

				var ir = new InfraredSensor(Inputs.Input4);
				var driveService1 = new DriveService(Outputs.OutputD, Outputs.OutputA, Logger);

				ir.SetIrRemote();
				soundManager.PlaySound("ready");

				while (true)
				{
					Thread.Sleep(100);
					if (Console.KeyAvailable)
					{
						var key = Console.ReadKey();
						if (key.Key == ConsoleKey.Escape)
							break;
					}

					int value0 = ir.GetInt();

					var driveState1 = new DriveState(value0);
					driveService1.Drive(driveState1);
				}
			}
			catch (Exception ex)
			{
				if(Logger != null)
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
