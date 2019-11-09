using System.Collections.Generic;
using EV3.Dev.Csharp.Constants;

namespace EV3.Dev.Csharp.Devices
{
	/// <summary> 
	/// A generic interface to read data from the system's power_supply class. 
	/// Uses the built-in legoev3-battery if none is specified.
	/// </summary>
	public class PowerSupply : Device
	{
		public PowerSupply(string name)
		{
			string strClassDir = global::System.IO.Path.Combine(SysRoot, "class", "power_supply");

			if (string.IsNullOrEmpty(name))
				name = "legoev3-battery";

			Connect(DeviceTypes.PowerSupply, strClassDir,
				name,
				new Dictionary<string, string[]>());
		}

		/// <summary> 
		/// The measured current that the battery is supplying (in microamps)
		/// </summary>
		public int MeasuredCurrent => GetAttrInt("current_now");

		/// <summary> 
		/// The measured voltage that the battery is supplying (in microvolts)
		/// </summary>
		public int MeasuredVoltage => GetAttrInt("voltage_now");

		/// <summary> 
		/// </summary>
		public int MaxVoltage => GetAttrInt("voltage_max_design");

		/// <summary> 
		/// </summary>
		public int MinVoltage => GetAttrInt("voltage_min_design");

		/// <summary> 
		/// </summary>
		public string Technology => GetAttrString("technology");

		/// <summary> 
		/// </summary>
		public string Type => GetAttrString("type");
	}
}