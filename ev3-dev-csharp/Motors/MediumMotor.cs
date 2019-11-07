namespace EV3.Dev.Csharp.Motors
{
	/// <summary> 
	/// EV3 medium servo motor
	/// </summary>
	public class MediumMotor : Motor
	{
		public MediumMotor(string port)
			: base(port, "lego-ev3-m-motor")
		{

		}
	}
}