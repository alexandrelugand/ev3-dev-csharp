using EV3.Dev.Csharp.System;

namespace EV3.Dev.Csharp.Sensors
{
	/// <summary> 
	/// A generic interface to control I2C-type EV3 sensors.
	/// </summary>
	public class I2CSensor : Sensor
	{
		public I2CSensor(string port)
			: base(port, new[] { Drivers.NxtI2CSensor })
		{
		}

		/// <summary> 
		/// Returns the firmware version of the sensor if available. Currently only
		/// I2C/NXT sensors support this.
		/// </summary>
		public string FwVersion => GetAttrString("fw_version");

		/// <summary> 
		/// Returns the polling period of the sensor in milliseconds. Writing sets the
		/// polling period. Setting to 0 disables polling. Minimum value is hard
		/// coded as 50 msec. Returns -EOPNOTSUPP if changing polling is not supported.
		/// Currently only I2C/NXT sensors support changing the polling period.
		/// </summary>
		public int PollMs
		{
			get => GetAttrInt("poll_ms");
			set => SetAttrInt("poll_ms", value);
		}
	}
}