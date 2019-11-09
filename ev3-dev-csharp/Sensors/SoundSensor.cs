using System;
using EV3.Dev.Csharp.Constants;

namespace EV3.Dev.Csharp.Sensors
{
	/// <summary> 
	/// LEGO NXT Sound Sensor
	/// </summary>
	public class SoundSensor : Sensor
	{
		public SoundSensor(string port)
			: base(port, DeviceTypes.Sound, new[] { Drivers.LegoNxtSound })
		{
			throw new NotImplementedException();
		}

		/// <summary> 
		/// Sound pressure level. Flat weighting
		/// </summary>
		public const string ModeDb = "DB";

		/// <summary> 
		/// Sound pressure level. A weighting
		/// </summary>
		public const string ModeDba = "DBA";
	}
}