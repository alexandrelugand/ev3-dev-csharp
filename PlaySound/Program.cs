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

				int value0 = s.GetInt();
				switch (value0)
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
							if (p != null && !p.HasExited)
								p.Kill();
							break;
						}
				}
			}

		}

		public static Process PlaySoundFile(Process p, string soundFilePath)
		{
			if(p != null && !p.HasExited)
				p.Kill();

			var proc = new Process
			{
				EnableRaisingEvents = false,
				StartInfo =
				{
					FileName = "aplay",
					Arguments = soundFilePath
				}
			};
			proc.Start();
			return proc;
		}
	}
}
