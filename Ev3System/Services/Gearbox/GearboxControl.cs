using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Motors;
using log4net;
using Prism.Events;
using System;
using System.Threading;

namespace Ev3System.Services.Gearbox
{
    public class GearboxControl : MarshalByRefObject, IGearboxControl
    {
        private readonly ILog _log;
        private readonly IEventAggregator _eventAggregator;
        private readonly ManualResetEvent _stopEvent;
        private readonly LargeMotor _motor;
        private Thread _thread;
        private bool _disposed;
        private const int ShiftAngle = 65;

        public GearboxControl
        (
            ILog log,
            IEventAggregator eventAggregator
        )
        {
            _log = log;
            _eventAggregator = eventAggregator;
            _stopEvent = new ManualResetEvent(false);

            _motor = new LargeMotor(Outputs.OutputB);
        }

        public override object InitializeLifetimeService() => null;

        public string ServiceName => nameof(GearboxControl);

        public void Prepare()
        {
            _log.Info("Prepare gearbox control...");
            Gear = 1;

            _motor.SpeedSp = 200;
            _motor.RunForever();
            _motor.StopCommand = Motor.StopCommandBrake;
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

        public void GearUp()
        {
            if (Gear == 2)
                return;

            var position = _motor.Position;
            _log.Info("Gear up...");
            _motor.Stop();
            _motor.PositionSp = ShiftAngle;
            _motor.RunToRelPos();

            Gear = 2;
        }

        public void GearDown()
        {
            if (Gear == 1)
                return;

            var position = _motor.Position;
            _log.Info("Gear down...");
            _motor.Stop();
            _motor.PositionSp = -ShiftAngle;
            _motor.RunToRelPos();

            Gear = 1;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = disposing;
            Unprepare();
            _motor?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
