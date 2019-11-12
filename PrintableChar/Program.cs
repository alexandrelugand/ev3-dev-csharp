using System;
using System.Threading;
using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Services;
using log4net;

namespace PrintableChar
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.Clear();
			Console.WriteLine("ASCII table");
			Console.WriteLine("#     0 1 2 3 4 5 6 7 8 9");
			for(var i = 2; i < 13; i++)
			{
				var line = $"#{i,2}  ";
				for (int j = 0; j < 10; j++)
				{
					var num = (i * 10 + j);
					if(num > 127)
						break;
					var c = (char)num;
					line += $"{c,2}";
				}
				Console.WriteLine(line);
			}

			Console.ReadKey();
			Console.Clear();
			Console.WriteLine("Extended ASCII table");
			Console.WriteLine("#     0 1 2 3 4 5 6 7 8 9");
			for (var i = 12; i < 26; i++)
			{
				var line = $"#{i,2}  ";
				for (int j = 0; j < 10; j++)
				{
					var num = (i * 10 + j);
					if (num > 255)
						break;
					if (num < 128)
						num = 32;
					var c = (char)num;
					line += $"{c,2}";
				}
				Console.WriteLine(line);
			}

			Console.ReadKey();
			Console.Clear();

			foreach (ConsoleColor consoleColor in Enum.GetValues(typeof(ConsoleColor)))
			{
				Console.BackgroundColor = consoleColor;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine($"[  OK  ]  A trace log {consoleColor} and {ConsoleColor.Black}");
			}
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;

			Console.ReadKey();
			Console.Clear();

			foreach (ConsoleColor consoleColor in Enum.GetValues(typeof(ConsoleColor)))
			{
				Console.BackgroundColor = ConsoleColor.Black; 
				Console.ForegroundColor = consoleColor;
				Console.WriteLine($"[  OK  ]  A trace log {ConsoleColor.Black} and {consoleColor}");
			}
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;

			Console.ReadKey();
			Console.Clear();

			var it = 0;
			Console.CursorVisible = false;
			Console.WriteLine("Increment value:");
			Console.Write("it = ");
			while (true)
			{
				Thread.Sleep(100);
				if (Console.KeyAvailable)
				{
					var key = Console.ReadKey();
					if (key.Key == ConsoleKey.Enter)
						break;
				}
				
				Console.Write($"{it++ % 100,3}");
				Console.SetCursorPosition(Console.CursorLeft - 3, 1);
			}

			Console.BackgroundColor = ConsoleColor.Black;
			Console.Clear();

			var logger = Ev3Services.Instance.GetService<ILog>();
			logger.Status(Status.OK, "A good status");
			logger.Status(Status.KO, "A bad status!");
			Console.ReadKey();


			Console.Clear();
		}
	}
}
