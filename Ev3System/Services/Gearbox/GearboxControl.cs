using System;
using System.Threading;
using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Motors;
using log4net;

namespace Ev3System.Services.Gearbox
{
	public class GearboxControl : MarshalByRefObject, IGearboxControl
	{
		private readonly ILog _log;
		private readonly MediumMotor _motor;
		private readonly ManualResetEvent _stopEvent;
		private bool _disposed;
		private Thread _thread;

		public GearboxControl
		(
			ILog log
		)
		{
			_log = log;
			_stopEvent = new ManualResetEvent(false);

			_motor = new MediumMotor(Outputs.OutputB);
			Gear = 1;
			Angle = 1050;
			Speed = 1100;
		}

		#region IGearboxControl Members

		public int Speed { get; set; }

		public string ServiceName => nameof(GearboxControl);

		public void Prepare()
		{
			_log.Info("Prepare gearbox control...");
			Gear = 1;

			_motor.SpeedSp = Speed;
			_motor.StopCommand = Motor.StopCommandCoast;
			_motor.Stop();

			_thread = new Thread(() =>
			{
				while (!_stopEvent.WaitOne(1000))
				{
				}
			});
			_thread.Start();
		}

		public void Unprepare()
		{
			_log.Info("Unprepare gearbox control...");
			GearDown();

			_motor.StopCommand = Motor.StopCommandCoast;
			_motor.Stop();

			if (_thread != null)
			{
				_stopEvent.Set();
				_thread.Join();
				_stopEvent.Reset();
				_thread = null;
			}
		}

		public int Gear { get; set; }

		public int Angle { get; set; }

		public void GearUp()
		{
			_log.Info("Gear up...");
			_motor.Stop();
			_motor.SpeedSp = Speed;
			_motor.PositionSp = -Angle;
			_motor.RunToRelPos();

			if (Gear < 5)
				Gear++;
		}

		public void GearDown()
		{
			_log.Info("Gear down...");
			_motor.Stop();
			_motor.SpeedSp = Speed;
			_motor.PositionSp = Angle;
			_motor.RunToRelPos();

			if (Gear > 1)
				Gear--;
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		public override object InitializeLifetimeService() => null;

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			_disposed = disposing;
			Unprepare();
			_motor?.Dispose();
		}
	}
}