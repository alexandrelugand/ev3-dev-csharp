using EV3.Dev.Csharp.Constants;

namespace EV3.Dev.Csharp.Sensors
{
	/// <summary> 
	/// LEGO NXT Light Sensor
	/// </summary>
	public class LightSensor : Sensor
	{
		public LightSensor(string port)
			: base(port, DeviceTypes.LightSensor, new[] { Drivers.LegoNxtLight })
		{
		}

		/// <summary> 
		/// Reflected light. LED on
		/// </summary>
		public const string ModeReflect = "REFLECT";

		/// <summary> 
		/// Ambient light. LED off
		/// </summary>
		public const string ModeAmbient = "AMBIENT";
	}
}