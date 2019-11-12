using EV3.Dev.Csharp.Motors;

namespace MotorControl.Commands
{
	public class CommandsLargeMotor : CommandsMotor
	{
		public override void Configure(string port)
		{
			Motor = new LargeMotor(ConfigurePort(port));
		}
	}
}
