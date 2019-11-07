using EV3.Dev.Csharp.System;

namespace EV3.Dev.Csharp.Sensors
{
	/// <summary> 
	/// Touch Sensor
	/// </summary>
	public class TouchSensor : Sensor
	{
		public TouchSensor(string port)
			: base(port, new[] { Drivers.LegoEv3Touch, Drivers.LegoNxtTouch })
		{
		}
	}
}