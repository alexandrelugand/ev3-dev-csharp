using System;
using EV3.Dev.Csharp.Services.Remoting;

namespace Ev3System.Services.Gearbox
{
	public interface IGearboxControl : IRemoteService, IDisposable
	{
		int Gear { get; }
		int Angle { get; set; }
		int Speed { get; set; }

		void Prepare();
		void Unprepare();

		void GearUp();
		void GearDown();
	}
}