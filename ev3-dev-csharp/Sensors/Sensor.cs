using System;
using System.Collections.Generic;
using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Devices;

namespace EV3.Dev.Csharp.Sensors
{
	/// <summary> 
	/// The sensor class provides a uniform interface for using most of the 
	/// sensors available for the EV3. The various underlying device drivers will 
	/// create a `lego-sensor` device for interacting with the sensors. 
	///  
	/// Sensors are primarily controlled by setting the `mode` and monitored by 
	/// reading the `value&lt;N&gt;` attributes. Values can be converted to floating point 
	/// if needed by `value&lt;N&gt;` / 10.0 ^ `decimals`. 
	///  
	/// Since the name of the `sensor&lt;N&gt;` device node does not correspond to the port 
	/// that a sensor is plugged in to, you must look at the `port_name` attribute if 
	/// you need to know which port a sensor is plugged in to. However, if you don't 
	/// have more than one sensor of each type, you can just look for a matching 
	/// `driver_name`. Then it will not matter which port a sensor is plugged in to - your 
	/// program will still work.
	/// </summary>
	public class Sensor : Device
	{
		public Sensor(string port, string deviceType)
		{
			Connect(deviceType, new Dictionary<string, string[]>
			{
				{ "address", new[] { port }
				}
			});
		}

		protected bool Connect(string deviceType, IDictionary<string, string[]> match)
		{
			string classDir = global::System.IO.Path.Combine(SysRoot, "class", "lego-sensor");
			string pattern = "sensor";

			return Connect(deviceType, classDir, pattern, match);
		}

		public Sensor(string port, string deviceType, string[] driverNames)
		{
			Connect(deviceType, new Dictionary<string, string[]>
			{
				{ "address", new[] { port } },
				{ "driver_name", driverNames }
			});
		}


		/// <summary>
		/// Returns the value or values measured by the sensor. Check `num_values` to
		/// see how many values there are. Values with index >= num_values will return
		/// an error. The values are fixed point numbers, so check `decimals` to see
		/// if you need to divide to get the actual value.
		/// </summary>
		public int GetInt(int index = 0)
		{
			if (index >= NumValues)
				throw new ArgumentOutOfRangeException();

			return GetAttrInt("value" + index);
		}

		/// <summary>
		/// The value converted to float using `decimals`.
		/// </summary>
		public double GetFloat(int index = 0)
		{
			return GetInt(index) * Math.Pow(10, -Decimals);
		}

		/// <summary>
		/// Human-readable name of the connected sensor.
		/// </summary>
		public string TypeName
		{
			get
			{
				var type = DriverName;
				if (string.IsNullOrEmpty(type))
				{
					return "<none>";
				}

				var lookupTable = new Dictionary<string, string>{
					{ Drivers.LegoEv3Touch,       "EV3 touch" },
					{ Drivers.LegoEv3Color,       "EV3 color" },
					{ Drivers.LegoEv3Us,  "EV3 ultrasonic" },
					{ Drivers.LegoEv3Gyro,        "EV3 gyro" },
					{ Drivers.LegoEv3Ir,    "EV3 infrared" },
					{ Drivers.LegoNxtTouch,       "NXT touch" },
					{ Drivers.LegoNxtLight,       "NXT light" },
					{ Drivers.LegoNxtSound,       "NXT sound" },
					{ Drivers.LegoNxtUs,  "NXT ultrasonic" },
					{ Drivers.NxtI2CSensor,  "I2C sensor" },
				};

				string value;
				if (lookupTable.TryGetValue(type, out value))
					return value;

				return type;
			}
		}

		/// <summary> 
		/// Sends a command to the sensor.
		/// </summary>
		public string Command
		{
			set => SetAttrString("command", value);
		}

		/// <summary> 
		/// Returns a list of the valid commands for the sensor.
		/// Returns -EOPNOTSUPP if no commands are supported.
		/// </summary>
		public string[] Commands => GetAttrSet("commands");

		/// <summary> 
		/// Returns the number of decimal places for the values in the `value&lt;N&gt;`
		/// attributes of the current mode.
		/// </summary>
		public int Decimals => GetAttrInt("decimals");

		/// <summary> 
		/// Returns the name of the sensor device/driver. See the list of [supported
		/// sensors] for a complete list of drivers.
		/// </summary>
		public string DriverName => GetAttrString("driver_name");

		/// <summary> 
		/// Returns the current mode. Writing one of the values returned by `modes`
		/// sets the sensor to that mode.
		/// </summary>
		public string Mode
		{
			get => GetAttrString("mode");
			set => SetAttrString("mode", value);
		}

		/// <summary> 
		/// Returns a list of the valid modes for the sensor.
		/// </summary>
		public string[] Modes => GetAttrSet("modes");

		/// <summary> 
		/// Returns the number of `value&lt;N&gt;` attributes that will return a valid value
		/// for the current mode.
		/// </summary>
		public int NumValues => GetAttrInt("num_values");

		/// <summary> 
		/// Returns the name of the port that the sensor is connected to, e.g. `ev3:in1`.
		/// I2C sensors also include the I2C address (decimal), e.g. `ev3:in1:i2c8`.
		/// </summary>
		public string PortName => GetAttrString("address");

		/// <summary> 
		/// Returns the units of the measured value for the current mode. May return
		/// empty string
		/// </summary>
		public string Units => GetAttrString("units");
	}
}