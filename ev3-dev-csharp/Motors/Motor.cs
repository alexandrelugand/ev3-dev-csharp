using System.Collections.Generic;
using EV3.Dev.Csharp.Devices;

namespace EV3.Dev.Csharp.Motors
{
	/// <summary> 
	/// The motor class provides a uniform interface for using motors with 
	/// positional and directional feedback such as the EV3 and NXT motors. 
	/// This feedback allows for precise control of the motors. This is the 
	/// most common type of motor, so we just call it `motor`.
	/// </summary>
	public class Motor : Device
	{
		protected Motor(string port, string deviceType, string motorType)
		{
			Connect(deviceType, new Dictionary<string, string[]>
			{
				{ "address", new[] { port } },
				{ "driver_name", new[] { motorType } }
			});
		}

		/// <summary> 
		/// Run the motor until another command is sent.
		/// </summary>
		public const string CommandRunForever = "run-forever";

		/// <summary> 
		/// Run to an absolute position specified by `position_sp` and then
		/// stop using the command specified in `stop_command`.
		/// </summary>
		public const string CommandRunToAbsPos = "run-to-abs-pos";

		/// <summary> 
		/// Run to a position relative to the current `position` value.
		/// The new position will be current `position` + `position_sp`.
		/// When the new position is reached, the motor will stop using
		/// the command specified by `stop_command`.
		/// </summary>
		public const string CommandRunToRelPos = "run-to-rel-pos";

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
		/// Reset all of the motor parameter attributes to their default value.
		/// This will also have the effect of stopping the motor.
		/// </summary>
		public const string CommandReset = "reset";

		/// <summary> 
		/// Sets the normal polarity of the rotary encoder.
		/// </summary>
		public const string EncoderPolarityNormal = "normal";

		/// <summary> 
		/// Sets the inversed polarity of the rotary encoder.
		/// </summary>
		public const string EncoderPolarityInversed = "inversed";

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
		/// The motor controller will vary the power supplied to the motor
		/// to try to maintain the speed specified in `speed_sp`.
		/// </summary>
		public const string SpeedRegulationOn = "on";

		/// <summary> 
		/// The motor controller will use the power specified in `duty_cycle_sp`.
		/// </summary>
		public const string SpeedRegulationOff = "off";

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
		/// Does not remove power from the motor. Instead it actively try to hold the motor
		/// at the current position. If an external force tries to turn the motor, the motor
		/// will ``push back`` to maintain its position.
		/// </summary>
		public const string StopCommandHold = "hold";

		protected bool Connect(string device, IDictionary<string, string[]> match)
		{
			string classDir = global::System.IO.Path.Combine(SysRoot, "class", "tacho-motor");
			string pattern = "motor";

			return Connect(device, classDir, pattern, match);
		}

		/// <summary> 
		/// Run the motor until another command is sent.
		/// </summary>        
		public void RunForever() { Command = "run-forever"; }

		/// <summary> 
		/// Run to an absolute position specified by `position_sp` and then
		/// stop using the command specified in `stop_command`.
		/// </summary>        
		public void RunToAbsPos() { Command = "run-to-abs-pos"; }

		/// <summary> 
		/// Run to a position relative to the current `position` value.
		/// The new position will be current `position` + `position_sp`.
		/// When the new position is reached, the motor will stop using
		/// the command specified by `stop_command`.
		/// </summary>        
		public void RunToRelPos() { Command = "run-to-rel-pos"; }

		/// <summary> 
		/// Run the motor for the amount of time specified in `time_sp`
		/// and then stop the motor using the command specified by `stop_command`.
		/// </summary>        
		public void RunTimed() { Command = "run-timed"; }

		/// <summary> 
		/// Run the motor at the duty cycle specified by `duty_cycle_sp`.
		/// Unlike other run commands, changing `duty_cycle_sp` while running *will*
		/// take effect immediately.
		/// </summary>        
		public void RunDirect() { Command = "run-direct"; }

		/// <summary> 
		/// Stop any of the run commands before they are complete using the
		/// command specified by `stop_command`.
		/// </summary>        
		public void Stop() { Command = "stop"; }

		/// <summary> 
		/// Reset all of the motor parameter attributes to their default value.
		/// This will also have the effect of stopping the motor.
		/// </summary>        
		public void Reset() { Command = "reset"; }

		/// <summary> 
		/// Sends a command to the motor controller. See `commands` for a list of
		/// possible values.
		/// </summary>
		public string Command
		{
			set => SetAttrString("command", value);
		}

		/// <summary> 
		/// Returns a list of commands that are supported by the motor
		/// controller. Possible values are `run-forever`, `run-to-abs-pos`, `run-to-rel-pos`,
		/// `run-timed`, `run-direct`, `stop` and `reset`. Not all commands may be supported.
		/// 
		/// - `run-forever` will cause the motor to run until another command is sent.
		/// - `run-to-abs-pos` will run to an absolute position specified by `position_sp`
		///   and then stop using the command specified in `stop_command`.
		/// - `run-to-rel-pos` will run to a position relative to the current `position` value.
		///   The new position will be current `position` + `position_sp`. When the new
		///   position is reached, the motor will stop using the command specified by `stop_command`.
		/// - `run-timed` will run the motor for the amount of time specified in `time_sp`
		///   and then stop the motor using the command specified by `stop_command`.
		/// - `run-direct` will run the motor at the duty cycle specified by `duty_cycle_sp`.
		///   Unlike other run commands, changing `duty_cycle_sp` while running *will*
		///   take effect immediately.
		/// - `stop` will stop any of the run commands before they are complete using the
		///   command specified by `stop_command`.
		/// - `reset` will reset all of the motor parameter attributes to their default value.
		///   This will also have the effect of stopping the motor.
		/// </summary>
		public string[] Commands => GetAttrSet("commands");

		/// <summary> 
		/// Returns the number of tacho counts in one rotation of the motor. Tacho counts
		/// are used by the position and speed attributes, so you can use this value
		/// to convert rotations or degrees to tacho counts. In the case of linear
		/// actuators, the units here will be counts per centimeter.
		/// </summary>
		public int CountPerRot => GetAttrInt("count_per_rot");

		/// <summary> 
		/// Returns the name of the driver that provides this tacho motor device.
		/// </summary>
		public string DriverName => GetAttrString("driver_name");

		/// <summary> 
		/// Returns the current duty cycle of the motor. Units are percent. Values
		/// are -100 to 100.
		/// </summary>
		public int DutyCycle => GetAttrInt("duty_cycle");

		/// <summary> 
		/// Writing sets the duty cycle setpoint. Reading returns the current value.
		/// Units are in percent. Valid values are -100 to 100. A negative value causes
		/// the motor to rotate in reverse. This value is only used when `speed_regulation`
		/// is off.
		/// </summary>
		public int DutyCycleSp
		{
			get => GetAttrInt("duty_cycle_sp");
			set => SetAttrInt("duty_cycle_sp", value);
		}

		/// <summary> 
		/// Sets the polarity of the rotary encoder. This is an advanced feature to all
		/// use of motors that send inversed encoder signals to the EV3. This should
		/// be set correctly by the driver of a device. It You only need to change this
		/// value if you are using a unsupported device. Valid values are `normal` and
		/// `inversed`.
		/// </summary>
		public string EncoderPolarity
		{
			get => GetAttrString("encoder_polarity");
			set => SetAttrString("encoder_polarity", value);
		}

		/// <summary> 
		/// Sets the polarity of the motor. With `normal` polarity, a positive duty
		/// cycle will cause the motor to rotate clockwise. With `inversed` polarity,
		/// a positive duty cycle will cause the motor to rotate counter-clockwise.
		/// Valid values are `normal` and `inversed`.
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
		/// Returns the current position of the motor in pulses of the rotary
		/// encoder. When the motor rotates clockwise, the position will increase.
		/// Likewise, rotating counter-clockwise causes the position to decrease.
		/// Writing will set the position to that value.
		/// </summary>
		public int Position
		{
			get => GetAttrInt("position");
			set => SetAttrInt("position", value);
		}

		/// <summary> 
		/// The proportional constant for the position PID.
		/// </summary>
		public int PositionP
		{
			get => GetAttrInt("hold_pid/Kp");
			set => SetAttrInt("hold_pid/Kp", value);
		}

		/// <summary> 
		/// The integral constant for the position PID.
		/// </summary>
		public int PositionI
		{
			get => GetAttrInt("hold_pid/Ki");
			set => SetAttrInt("hold_pid/Ki", value);
		}

		/// <summary> 
		/// The derivative constant for the position PID.
		/// </summary>
		public int PositionD
		{
			get => GetAttrInt("hold_pid/Kd");
			set => SetAttrInt("hold_pid/Kd", value);
		}

		/// <summary> 
		/// Writing specifies the target position for the `run-to-abs-pos` and `run-to-rel-pos`
		/// commands. Reading returns the current value. Units are in tacho counts. You
		/// can use the value returned by `counts_per_rot` to convert tacho counts to/from
		/// rotations or degrees.
		/// </summary>
		public int PositionSp
		{
			get => GetAttrInt("position_sp");
			set => SetAttrInt("position_sp", value);
		}

		/// <summary> 
		/// Returns the current motor speed in tacho counts per second. Not, this is
		/// not necessarily degrees (although it is for LEGO motors). Use the `count_per_rot`
		/// attribute to convert this value to RPM or deg/sec.
		/// </summary>
		public int Speed => GetAttrInt("speed");

		/// <summary> 
		/// Writing sets the target speed in tacho counts per second used when `speed_regulation`
		/// is on. Reading returns the current value.  Use the `count_per_rot` attribute
		/// to convert RPM or deg/sec to tacho counts per second.
		/// </summary>
		public int SpeedSp
		{
			get => GetAttrInt("speed_sp");
			set => SetAttrInt("speed_sp", value);
		}

		/// <summary> 
		/// Writing sets the ramp up setpoint. Reading returns the current value. Units
		/// are in milliseconds. When set to a value > 0, the motor will ramp the power
		/// sent to the motor from 0 to 100% duty cycle over the span of this setpoint
		/// when starting the motor. If the maximum duty cycle is limited by `duty_cycle_sp`
		/// or speed regulation, the actual ramp time duration will be less than the setpoint.
		/// </summary>
		public int RampUpSp
		{
			get => GetAttrInt("ramp_up_sp");
			set => SetAttrInt("ramp_up_sp", value);
		}

		/// <summary> 
		/// Writing sets the ramp down setpoint. Reading returns the current value. Units
		/// are in milliseconds. When set to a value > 0, the motor will ramp the power
		/// sent to the motor from 100% duty cycle down to 0 over the span of this setpoint
		/// when stopping the motor. If the starting duty cycle is less than 100%, the
		/// ramp time duration will be less than the full span of the setpoint.
		/// </summary>
		public int RampDownSp
		{
			get => GetAttrInt("ramp_down_sp");
			set => SetAttrInt("ramp_down_sp", value);
		}

		/// <summary> 
		/// Turns speed regulation on or off. If speed regulation is on, the motor
		/// controller will vary the power supplied to the motor to try to maintain the
		/// speed specified in `speed_sp`. If speed regulation is off, the controller
		/// will use the power specified in `duty_cycle_sp`. Valid values are `on` and
		/// `off`.
		/// </summary>
		public string SpeedRegulationEnabled
		{
			get => GetAttrString("speed_regulation");
			set => SetAttrString("speed_regulation", value);
		}

		/// <summary> 
		/// The proportional constant for the speed regulation PID.
		/// </summary>
		public int SpeedRegulationP
		{
			get => GetAttrInt("speed_pid/Kp");
			set => SetAttrInt("speed_pid/Kp", value);
		}

		/// <summary> 
		/// The integral constant for the speed regulation PID.
		/// </summary>
		public int SpeedRegulationI
		{
			get => GetAttrInt("speed_pid/Ki");
			set => SetAttrInt("speed_pid/Ki", value);
		}

		/// <summary> 
		/// The derivative constant for the speed regulation PID.
		/// </summary>
		public int SpeedRegulationD
		{
			get => GetAttrInt("speed_pid/Kd");
			set => SetAttrInt("speed_pid/Kd", value);
		}

		/// <summary> 
		/// Reading returns a list of state flags. Possible flags are
		/// `running`, `ramping` `holding` and `stalled`.
		/// </summary>
		public string[] State => GetAttrSet("state");

		/// <summary> 
		/// Reading returns the current stop command. Writing sets the stop command.
		/// The value determines the motors behavior when `command` is set to `stop`.
		/// Also, it determines the motors behavior when a run command completes. See
		/// `stop_commands` for a list of possible values.
		/// </summary>
		public string StopCommand
		{
			get => GetAttrString("stop_action");
			set => SetAttrString("stop_action", value);
		}

		/// <summary> 
		/// Returns a list of stop modes supported by the motor controller.
		/// Possible values are `coast`, `brake` and `hold`. `coast` means that power will
		/// be removed from the motor and it will freely coast to a stop. `brake` means
		/// that power will be removed from the motor and a passive electrical load will
		/// be placed on the motor. This is usually done by shorting the motor terminals
		/// together. This load will absorb the energy from the rotation of the motors and
		/// cause the motor to stop more quickly than coasting. `hold` does not remove
		/// power from the motor. Instead it actively try to hold the motor at the current
		/// position. If an external force tries to turn the motor, the motor will 'push
		/// back' to maintain its position.
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