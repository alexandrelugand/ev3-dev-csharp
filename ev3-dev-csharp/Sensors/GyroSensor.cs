using EV3.Dev.Csharp.System;

namespace EV3.Dev.Csharp.Sensors
{
	/// <summary> 
	/// LEGO EV3 gyro sensor.
	/// </summary>
	public class GyroSensor : Sensor
	{
		public GyroSensor(string port)
			: base(port, new[] { Drivers.LegoEv3Gyro })
		{
		}

		/// <summary> 
		/// Angle
		/// </summary>
		public const string ModeGyroAng = "GYRO-ANG";

		/// <summary> 
		/// Rotational speed
		/// </summary>
		public const string ModeGyroRate = "GYRO-RATE";

		/// <summary> 
		/// Raw sensor value
		/// </summary>
		public const string ModeGyroFas = "GYRO-FAS";

		/// <summary> 
		/// Angle and rotational speed
		/// </summary>
		public const string ModeGyroGAnda = "GYRO-G&A";

		/// <summary> 
		/// Calibration ???
		/// </summary>
		public const string ModeGyroCal = "GYRO-CAL";
	}
}