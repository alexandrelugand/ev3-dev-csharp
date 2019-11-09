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

namespace PlaySound
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
				Logger.Debug($"###### EV3 Play sound ({version}) ######");

				var soundManager = ev3Services.GetService<ISoundManager>();
				var s = new InfraredSensor(Inputs.Input4);
				while (true)
				{
					Thread.Sleep(100);
					if (Console.KeyAvailable)
					{
						var key = Console.ReadKey();
						if (key.Key == ConsoleKey.Escape)
							break;
					}

					switch (s.GetInt())
					{
						case 1: // red up
							soundManager.PlaySoundFile("sound1.rsf");
							break;
						case 3: // blue up
							soundManager.PlaySoundFile("sound2.rsf");
							break;
						case 2:// red down
							soundManager.PlaySoundFile("sound3.rsf");
							break;
						case 4:// blue down
							soundManager.PlaySoundFile("sound4.rsf");
							break;
						case 9:
						{
							soundManager.InterruptSound();
							break;
						}
					}
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
