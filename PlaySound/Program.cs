using System;
using System.Diagnostics;
using System.Threading;
using EV3.Dev.Csharp.Sensors;
using EV3.Dev.Csharp.System;

namespace PlaySound
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.Clear();
			var s = new InfraredSensor(Inputs.Input4);
			Process p = null;
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
						p = PlaySoundFile(p, "sound1.rsf");
						break;
					case 3: // blue up
						p = PlaySoundFile(p, "sound2.rsf");
						break;
					case 2:// red down
						p = PlaySoundFile(p, "sound3.rsf");
						break;
					case 4:// blue down
						p = PlaySoundFile(p, "sound4.rsf");
						break;
					case 9:
						{
							InterrutSound(p);
							break;
						}
				}
			}

		}

		public static void InterrutSound(Process p)
		{
			if (p != null && !p.HasExited)
			{
				p.EnableRaisingEvents = false;
				p.Kill();
				Console.WriteLine("Interrupt sound!");
			}
		}

		public static Process PlaySoundFile(Process p, string soundFilePath)
		{
			InterrutSound(p);
			var proc = new Process
			{
				EnableRaisingEvents = true,
				StartInfo =
				{
					FileName = "aplay",
					Arguments = soundFilePath
				}
			};
			proc.Start();
			proc.Exited += (sender, args) => Console.WriteLine($"'{soundFilePath}' sound finished");
			Console.WriteLine($"Play '{soundFilePath}' sound...");
			return proc;
		}
	}
}
