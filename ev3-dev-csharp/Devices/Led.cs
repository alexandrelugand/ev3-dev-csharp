using System;

namespace EV3.Dev.Csharp.Devices
{
	/// <summary> 
	/// Any device controlled by the generic LED driver. 
	/// See https://www.kernel.org/doc/Documentation/leds/leds-class.txt 
	/// for more details.
	/// </summary>
	public class Led : Device
	{
		public Led()
		{
			throw new NotImplementedException();
		}

		/// <summary> 
		/// Returns the maximum allowable brightness value.
		/// </summary>
		public int MaxBrightness => GetAttrInt("max_brightness");

		/// <summary> 
		/// Sets the brightness level. Possible values are from 0 to `max_brightness`.
		/// </summary>
		public int Brightness
		{
			get => GetAttrInt("brightness");
			set => SetAttrInt("brightness", value);
		}

		/// <summary> 
		/// Returns a list of available triggers.
		/// </summary>
		public string[] Triggers => GetAttrSet("trigger");

		/// <summary> 
		/// Sets the led trigger. A trigger
		/// is a kernel based source of led events. Triggers can either be simple or
		/// complex. A simple trigger isn't configurable and is designed to slot into
		/// existing subsystems with minimal additional code. Examples are the `ide-disk` and
		/// `nand-disk` triggers.
		/// 
		/// Complex triggers whilst available to all LEDs have LED specific
		/// parameters and work on a per LED basis. The `timer` trigger is an example.
		/// The `timer` trigger will periodically change the LED brightness between
		/// 0 and the current brightness setting. The `on` and `off` time can
		/// be specified via `delay_{on,off}` attributes in milliseconds.
		/// You can change the brightness value of a LED independently of the timer
		/// trigger. However, if you set the brightness value to 0 it will
		/// also disable the `timer` trigger.
		/// </summary>
		public string Trigger
		{
			get => GetAttrFromSet("trigger");
			set => SetAttrString("trigger", value);
		}

		/// <summary> 
		/// The `timer` trigger will periodically change the LED brightness between
		/// 0 and the current brightness setting. The `on` time can
		/// be specified via `delay_on` attribute in milliseconds.
		/// </summary>
		public int DelayOn
		{
			get => GetAttrInt("delay_on");
			set => SetAttrInt("delay_on", value);
		}

		/// <summary> 
		/// The `timer` trigger will periodically change the LED brightness between
		/// 0 and the current brightness setting. The `off` time can
		/// be specified via `delay_off` attribute in milliseconds.
		/// </summary>
		public int DelayOff
		{
			get => GetAttrInt("delay_off");
			set => SetAttrInt("delay_off", value);
		}
	}
}