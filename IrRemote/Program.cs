/*
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 *
 */

using System;
using System.Threading;
using EV3.Dev.Csharp;
using EV3.Dev.Csharp.Sensors;
using EV3.Dev.Csharp.System;

namespace IrRemote
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.Clear();
			
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
	}
}
