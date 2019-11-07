using EV3.Dev.Csharp.System;

namespace EV3.Dev.Csharp.Sensors
{
	/// <summary> 
	/// LEGO EV3 infrared sensor.
	/// </summary>
	public class InfraredSensor : Sensor
	{
		public InfraredSensor(string port)
			: base(port, new[] { Drivers.LegoEv3Ir })
		{

		}

		/// <summary> 
		/// Proximity
		/// </summary>
		public const string ModeIrProx = "IR-PROX";

		/// <summary> 
		/// IR Seeker
		/// </summary>
		public const string ModeIrSeek = "IR-SEEK";

		/// <summary> 
		/// IR Remote Control
		/// </summary>
		public const string ModeIrRemote = "IR-REMOTE";

		/// <summary> 
		/// IR Remote Control. State of the buttons is coded in binary
		/// </summary>
		public const string ModeIrRemA = "IR-REM-A";

		/// <summary> 
		/// Calibration ???
		/// </summary>
		public const string ModeIrCal = "IR-CAL";

		/// <summary> 
		/// Proximity
		/// </summary>        
		public void SetIrProx() { Mode = "IR-PROX"; }
		public bool IsIrProx() { return Mode == "IR-PROX"; }

		/// <summary> 
		/// IR Seeker
		/// </summary>        
		public void SetIrSeek() { Mode = "IR-SEEK"; }
		public bool IsIrSeek() { return Mode == "IR-SEEK"; }

		/// <summary> 
		/// IR Remote Control
		/// </summary>        
		public void SetIrRemote() { Mode = "IR-REMOTE"; }
		public bool IsIrRemote() { return Mode == "IR-REMOTE"; }

		/// <summary> 
		/// IR Remote Control. State of the buttons is coded in binary
		/// </summary>        
		public void SetIrRemA() { Mode = "IR-REM-A"; }
		public bool IsIrRemA() { return Mode == "IR-REM-A"; }

		/// <summary> 
		/// Calibration ???
		/// </summary>        
		public void SetIrCal() { Mode = "IR-CAL"; }
		public bool IsIrCal() { return Mode == "IR-CAL"; }
	}
}