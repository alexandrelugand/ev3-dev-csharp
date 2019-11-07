using System;
using EV3.Dev.Csharp.Devices;

namespace EV3.Dev.Csharp.Motors
{
	/// <summary> 
	/// The servo motor class provides a uniform interface for using hobby type 
	/// servo motors.
	/// </summary>
	public class ServoMotor : Device
	{
		public ServoMotor()
		{
			throw new NotImplementedException();
		}

		/// <summary> 
		/// Drive servo to the position set in the `position_sp` attribute.
		/// </summary>
		public const string CommandRun = "run";

		/// <summary> 
		/// Remove power from the motor.
		/// </summary>
		public const string CommandFloat = "float";

		/// <summary> 
		/// With `normal` polarity, a positive duty cycle will
		/// cause the motor to rotate clockwise.
		/// </summary>
		public const string PolarityNormal = "normal";

		/// <summary> 
		/// With `inversed` polarity, a positive duty cycle will
		/// cause the motor to rotate counter-clockwise.
		/// </summary>
		public const string PolarityInversed = "inversed";

		/// <summary> 
		/// Sets the command for the servo. Valid values are `run` and `float`. Setting
		/// to `run` will cause the servo to be driven to the position_sp set in the
		/// `position_sp` attribute. Setting to `float` will remove power from the motor.
		/// </summary>
		public string Command
		{
			set => SetAttrString("command", value);
		}

		/// <summary> 
		/// Returns the name of the motor driver that loaded this device. See the list
		/// of [supported devices] for a list of drivers.
		/// </summary>
		public string DriverName => GetAttrString("driver_name");

		/// <summary> 
		/// Used to set the pulse size in milliseconds for the signal that tells the
		/// servo to drive to the maximum (clockwise) position_sp. Default value is 2400.
		/// Valid values are 2300 to 2700. You must write to the position_sp attribute for
		/// changes to this attribute to take effect.
		/// </summary>
		public int MaxPulseSp
		{
			get => GetAttrInt("max_pulse_sp");
			set => SetAttrInt("max_pulse_sp", value);
		}

		/// <summary> 
		/// Used to set the pulse size in milliseconds for the signal that tells the
		/// servo to drive to the mid position_sp. Default value is 1500. Valid
		/// values are 1300 to 1700. For example, on a 180 degree servo, this would be
		/// 90 degrees. On continuous rotation servo, this is the 'neutral' position_sp
		/// where the motor does not turn. You must write to the position_sp attribute for
		/// changes to this attribute to take effect.
		/// </summary>
		public int MidPulseSp
		{
			get => GetAttrInt("mid_pulse_sp");
			set => SetAttrInt("mid_pulse_sp", value);
		}

		/// <summary> 
		/// Used to set the pulse size in milliseconds for the signal that tells the
		/// servo to drive to the miniumum (counter-clockwise) position_sp. Default value
		/// is 600. Valid values are 300 to 700. You must write to the position_sp
		/// attribute for changes to this attribute to take effect.
		/// </summary>
		public int MinPulseSp
		{
			get => GetAttrInt("min_pulse_sp");
			set => SetAttrInt("min_pulse_sp", value);
		}

		/// <summary> 
		/// Sets the polarity of the servo. Valid values are `normal` and `inversed`.
		/// Setting the value to `inversed` will cause the position_sp value to be
		/// inversed. i.e `-100` will correspond to `max_pulse_sp`, and `100` will
		/// correspond to `min_pulse_sp`.
		/// </summary>
		public string Polarity
		{
			get => GetAttrString("polarity");
			set => SetAttrString("polarity", value);
		}

		/// <summary> 
		/// Returns the name of the port that the motor is connected to.
		/// </summary>
		public string PortName => GetAttrString("address");

		/// <summary> 
		/// Reading returns the current position_sp of the servo. Writing instructs the
		/// servo to move to the specified position_sp. Units are percent. Valid values
		/// are -100 to 100 (-100% to 100%) where `-100` corresponds to `min_pulse_sp`,
		/// `0` corresponds to `mid_pulse_sp` and `100` corresponds to `max_pulse_sp`.
		/// </summary>
		public int PositionSp
		{
			get => GetAttrInt("position_sp");
			set => SetAttrInt("position_sp", value);
		}

		/// <summary> 
		/// Sets the rate_sp at which the servo travels from 0 to 100.0% (half of the full
		/// range of the servo). Units are in milliseconds. Example: Setting the rate_sp
		/// to 1000 means that it will take a 180 degree servo 2 second to move from 0
		/// to 180 degrees. Note: Some servo controllers may not support this in which
		/// case reading and writing will fail with `-EOPNOTSUPP`. In continuous rotation
		/// servos, this value will affect the rate_sp at which the speed ramps up or down.
		/// </summary>
		public int RateSp
		{
			get => GetAttrInt("rate_sp");
			set => SetAttrInt("rate_sp", value);
		}

		/// <summary> 
		/// Returns a list of flags indicating the state of the servo.
		/// Possible values are:
		/// * `running`: Indicates that the motor is powered.
		/// </summary>
		public string[] State => GetAttrSet("state");
	}
}