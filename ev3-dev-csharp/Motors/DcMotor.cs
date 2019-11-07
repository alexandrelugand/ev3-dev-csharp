using System;
using EV3.Dev.Csharp.Devices;

namespace EV3.Dev.Csharp.Motors
{
	/// <summary> 
	/// The DC motor class provides a uniform interface for using regular DC motors 
	/// with no fancy controls or feedback. This includes LEGO MINDSTORMS RCX motors 
	/// and LEGO Power Functions motors.
	/// </summary>
	public class DcMotor : Device
	{
		public DcMotor()
		{
			throw new NotImplementedException();
		}

		/// <summary> 
		/// Run the motor until another command is sent.
		/// </summary>
		public const string CommandRunForever = "run-forever";

		/// <summary> 
		/// Run the motor for the amount of time specified in `time_sp`
		/// and then stop the motor using the command specified by `stop_command`.
		/// </summary>
		public const string CommandRunTimed = "run-timed";

		/// <summary> 
		/// Run the motor at the duty cycle specified by `duty_cycle_sp`.
		/// Unlike other run commands, changing `duty_cycle_sp` while running *will*
		/// take effect immediately.
		/// </summary>
		public const string CommandRunDirect = "run-direct";

		/// <summary> 
		/// Stop any of the run commands before they are complete using the
		/// command specified by `stop_command`.
		/// </summary>
		public const string CommandStop = "stop";

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
		/// Power will be removed from the motor and it will freely coast to a stop.
		/// </summary>
		public const string StopCommandCoast = "coast";

		/// <summary> 
		/// Power will be removed from the motor and a passive electrical load will
		/// be placed on the motor. This is usually done by shorting the motor terminals
		/// together. This load will absorb the energy from the rotation of the motors and
		/// cause the motor to stop more quickly than coasting.
		/// </summary>
		public const string StopCommandBrake = "brake";

		/// <summary> 
		/// Sets the command for the motor. Possible values are `run-forever`, `run-timed` and
		/// `stop`. Not all commands may be supported, so be sure to check the contents
		/// of the `commands` attribute.
		/// </summary>
		public string Command
		{
			set => SetAttrString("command", value);
		}

		/// <summary> 
		/// Returns a list of commands supported by the motor
		/// controller.
		/// </summary>
		public string[] Commands => GetAttrSet("commands");

		/// <summary> 
		/// Returns the name of the motor driver that loaded this device. See the list
		/// of [supported devices] for a list of drivers.
		/// </summary>
		public string DriverName => GetAttrString("driver_name");

		/// <summary> 
		/// Shows the current duty cycle of the PWM signal sent to the motor. Values
		/// are -100 to 100 (-100% to 100%).
		/// </summary>
		public int DutyCycle => GetAttrInt("duty_cycle");

		/// <summary> 
		/// Writing sets the duty cycle setpoint of the PWM signal sent to the motor.
		/// Valid values are -100 to 100 (-100% to 100%). Reading returns the current
		/// setpoint.
		/// </summary>
		public int DutyCycleSp
		{
			get => GetAttrInt("duty_cycle_sp");
			set => SetAttrInt("duty_cycle_sp", value);
		}

		/// <summary> 
		/// Sets the polarity of the motor. Valid values are `normal` and `inversed`.
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
		/// Sets the time in milliseconds that it take the motor to ramp down from 100%
		/// to 0%. Valid values are 0 to 10000 (10 seconds). Default is 0.
		/// </summary>
		public int RampDownSp
		{
			get => GetAttrInt("ramp_down_sp");
			set => SetAttrInt("ramp_down_sp", value);
		}

		/// <summary> 
		/// Sets the time in milliseconds that it take the motor to up ramp from 0% to
		/// 100%. Valid values are 0 to 10000 (10 seconds). Default is 0.
		/// </summary>
		public int RampUpSp
		{
			get => GetAttrInt("ramp_up_sp");
			set => SetAttrInt("ramp_up_sp", value);
		}

		/// <summary> 
		/// Gets a list of flags indicating the motor status. Possible
		/// flags are `running` and `ramping`. `running` indicates that the motor is
		/// powered. `ramping` indicates that the motor has not yet reached the
		/// `duty_cycle_sp`.
		/// </summary>
		public string[] State => GetAttrSet("state");

		/// <summary> 
		/// Sets the stop command that will be used when the motor stops. Read
		/// `stop_commands` to get the list of valid values.
		/// </summary>
		public string StopCommand
		{
			set => SetAttrString("stop_command", value);
		}

		/// <summary> 
		/// Gets a list of stop commands. Valid values are `coast`
		/// and `brake`.
		/// </summary>
		public string[] StopCommands => GetAttrSet("stop_commands");

		/// <summary> 
		/// Writing specifies the amount of time the motor will run when using the
		/// `run-timed` command. Reading returns the current value. Units are in
		/// milliseconds.
		/// </summary>
		public int TimeSp
		{
			get => GetAttrInt("time_sp");
			set => SetAttrInt("time_sp", value);
		}
	}
}