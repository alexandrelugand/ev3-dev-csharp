using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Motors;
using EV3.Dev.Csharp.Sensors;
using log4net;

namespace HeadControl
{
	public class HeadControl
	{
		private readonly ILog _logger;
		private readonly InfraredSensor _ir;
		private readonly Motor _motor;
		private int _speed = 75;
		private int _couple = 5;
		private const int SecurityMarge = 10;

		public HeadControl(string irPort, string motorPort, ILog logger)
		{
			_logger = logger;
			_ir = new InfraredSensor(irPort);
			_motor = new MediumMotor(motorPort)
			{
				SpeedSp = 0,
				StopCommand = Motor.StopCommandCoast
			};
			Stop();
			_ir.SetIrProx();
		}

		private void Stop()
		{
			_motor.Stop();
			_motor.StopCommand = Motor.StopCommandCoast;

		}

		public void TurnLeft()
		{
			_motor.SpeedSp = _speed;
			_motor.DutyCycleSp = _couple;
			_motor.RunForever();
		}

		public void TurnLeftToPos(int pos)
		{
			_motor.SpeedSp = _speed;
			_motor.DutyCycleSp = _couple;
			_motor.PositionSp = pos;
			_motor.RunToAbsPos();
			while(!_motor.Position.IsWithin(pos - SecurityMarge, pos + SecurityMarge))
				Thread.Sleep(50);
		}

		public void TurnRight()
		{
			_motor.SpeedSp = -_speed;
			_motor.DutyCycleSp = -_couple;
			_motor.RunForever();
		}

		public void TurnRightToPos(int pos)
		{
			_motor.SpeedSp = -_speed;
			_motor.DutyCycleSp = -_couple;
			_motor.PositionSp = pos;
			_motor.RunToAbsPos();
			while (!_motor.Position.IsWithin(pos - SecurityMarge, pos + SecurityMarge))
				Thread.Sleep(50);
		}

		public int LeftStopPosition { get; set; }
		public int RightStopPosition { get; set; }
		public int MidStopPosition { get; set; }

		public void Calibrate()
		{
			//Turn Left
			_logger.Debug("Search left stop position...");
			TurnLeft();
			Thread.Sleep(250);
			while (true)
			{
				if (_motor.DutyCycle > 50)
				{
					Stop();
					LeftStopPosition = _motor.Position - SecurityMarge;
					_logger.Debug($"Left stop position: {LeftStopPosition}");
					break;
				}
				Thread.Sleep(50);
			}

			_logger.Debug("Search right stop position...");
			TurnRight();
			Thread.Sleep(250);
			while (true)
			{
				if (_motor.DutyCycle < -50)
				{
					Stop();
					RightStopPosition = _motor.Position + SecurityMarge;
					_logger.Debug($"Right stop position: {RightStopPosition}");
					break;
				}
				Thread.Sleep(50);
			}

			MidStopPosition = (int) ((LeftStopPosition + RightStopPosition) / 2);
			_logger.Debug($"Mid stop position: {MidStopPosition}");

			_logger.Debug("Returns to mid position...");
			TurnLeftToPos(MidStopPosition);
			Stop();
			Thread.Sleep(1000);

			_logger.Debug("Increase speed...");
			_speed *= 2;
			for (int i = 0; i < 3; i++)
			{
				TurnLeftToPos(LeftStopPosition);
				Stop();
				TurnRightToPos(RightStopPosition);
				Stop();
				TurnLeftToPos(MidStopPosition);
				Stop();
			}

			_logger.Status(Status.OK, "Calibration ended");
			Console.ReadKey();

		}

		public void Update()
		{
			Console.Write($"{_ir.GetInt(), 3}%");
			Console.CursorLeft -= 4;
		}
	}
}
