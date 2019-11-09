using EV3.Dev.Csharp.Constants;

namespace EV3.Dev.Csharp.Sensors
{
	/// <summary> 
	/// LEGO EV3 ultrasonic sensor.
	/// </summary>
	public class UltrasonicSensor : Sensor
	{
		public UltrasonicSensor(string port)
			: base(port, DeviceTypes.UltraSonicSensor, new[] { Drivers.LegoEv3Us, Drivers.LegoNxtUs })
		{
		}
		/// <summary> 
		/// Continuous measurement in centimeters.
		/// LEDs: On, steady
		/// </summary>
		public const string ModeUsDistCm = "US-DIST-CM";

		/// <summary> 
		/// Continuous measurement in inches.
		/// LEDs: On, steady
		/// </summary>
		public const string ModeUsDistIn = "US-DIST-IN";

		/// <summary> 
		/// Listen.  LEDs: On, blinking
		/// </summary>
		public const string ModeUsListen = "US-LISTEN";

		/// <summary> 
		/// Single measurement in centimeters.
		/// LEDs: On momentarily when mode is set, then off
		/// </summary>
		public const string ModeUsSiCm = "US-SI-CM";

		/// <summary> 
		/// Single measurement in inches.
		/// LEDs: On momentarily when mode is set, then off
		/// </summary>
		public const string ModeUsSiIn = "US-SI-IN";

	}
}