using System;
using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Motors;

namespace MotorControl.Commands
{
    public abstract class CommandsMotor
    {
	    protected static Motor Motor;

	    protected string ConfigurePort(string port)
	    {
		    switch (port.ToUpper())
		    {
			    case "A":
				    return Outputs.OutputA;

			    case "B":
				    return Outputs.OutputB;

			    case "C":
				    return Outputs.OutputC;

			    case "D":
				    return Outputs.OutputD;

			    default:
				    Console.WriteLine("Unknown port");
				    return string.Empty;
		    }
	    }

	    public abstract void Configure(string port);

		public void SetSpeed(int speed)
		{
			if (Motor == null)
			{
				Console.WriteLine("Configure medium motor before");
				return;
			}
			Motor.SpeedSp = speed;
		}

		public int GetSpeed()
		{
			if (Motor == null)
				return 0;
			return Motor.Speed;
		}

		public void SetCouple(int dutyCycle)
		{
			if (Motor == null)
			{
				Console.WriteLine("Configure medium motor before");
				return;
			}
			Motor.DutyCycleSp = dutyCycle;
		}

		public int GetCouple()
		{
			if (Motor == null)
				return 0;
			return Motor.DutyCycleSp;
		}

		public void Run()
		{
			if (Motor == null)
			{
				Console.WriteLine("Configure medium motor before");
				return;
			}
			Motor.RunForever();
		}

		public void Stop(bool brake = false)
		{
			if (Motor == null)
			{
				Console.WriteLine("Configure medium motor before");
				return;
			}
			Motor.Stop();
			Motor.StopCommand = brake ? Motor.StopCommandBrake : Motor.StopCommandCoast;
		}
	}
}
