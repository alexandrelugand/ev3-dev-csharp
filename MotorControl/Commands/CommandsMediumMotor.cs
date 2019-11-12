using EV3.Dev.Csharp.Motors;

namespace MotorControl.Commands
{
    public class CommandsMediumMotor : CommandsMotor
    {
		public override void Configure(string port)
		{
			Motor = new MediumMotor(ConfigurePort(port));
		}
	}
}
