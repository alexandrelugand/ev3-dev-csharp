using EV3.Dev.Csharp.Constants;

namespace EV3.Dev.Csharp.Sensors
{
	/// <summary> 
	/// LEGO EV3 color sensor.
	/// </summary>
	public class ColorSensor : Sensor
	{
		public ColorSensor(string port)
			: base(port, DeviceTypes.ColorSensor, new[] { Drivers.LegoEv3Color })
		{
		}

		/// <summary> 
		/// Reflected light. Red LED on.
		/// </summary>
		public const string ModeColReflect = "COL-REFLECT";

		/// <summary> 
		/// Ambient light. Red LEDs off.
		/// </summary>
		public const string ModeColAmbient = "COL-AMBIENT";

		/// <summary> 
		/// Color. All LEDs rapidly cycling, appears white.
		/// </summary>
		public const string ModeColColor = "COL-COLOR";

		/// <summary> 
		/// Raw reflected. Red LED on
		/// </summary>
		public const string ModeRefRaw = "REF-RAW";

		/// <summary> 
		/// Raw Color Components. All LEDs rapidly cycling, appears white.
		/// </summary>
		public const string ModeRgbRaw = "RGB-RAW";
	}
}