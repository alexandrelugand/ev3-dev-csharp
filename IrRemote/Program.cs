using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Sensors;
using EV3.Dev.Csharp.Services;
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
				Logger = Ev3Services.Instance.GetService<ILog>();
				Logger.Clear();

				var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
				var version = fvi.FileVersion;
				Logger.Debug($"###### EV3 Infrared Remote ({version}) ######");

				var s = new InfraredSensor(Inputs.Input4);

				var driveService1 = new DriveService(Outputs.OutputD, Outputs.OutputA);

				s.SetIrRemote();

				while (true)
				{
					Thread.Sleep(100);
					if (Console.KeyAvailable)
					{
						var key = Console.ReadKey();
						if (key.Key == ConsoleKey.Escape)
							break;
					}

					int value0 = s.GetInt();

					var driveState1 = new DriveState(value0);
					driveService1.Drive(driveState1);
				}
			}
			catch (Exception ex)
			{
				Logger?.Error("Unexpected error", ex);
				Console.ReadKey();
			}
		}
	}
}
