using EV3.Dev.Csharp.Constants;

namespace EV3.Dev.Csharp.Motors
{
	/// <summary> 
	/// EV3 large servo motor
	/// </summary>
	public class LargeMotor : Motor
	{
		public LargeMotor(string port)
			: base(port, DeviceTypes.LargeMotor, "lego-ev3-l-motor")
		{
		}
	}
}