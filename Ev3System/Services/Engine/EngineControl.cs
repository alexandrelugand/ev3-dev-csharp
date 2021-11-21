using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Events;
using EV3.Dev.Csharp.Motors;
using EV3.Dev.Csharp.Sensors;
using log4net;
using Prism.Events;
using System;
using System.Linq;
using System.Threading;

namespace Ev3System.Services.Engine
{
    public class EngineControl : MarshalByRefObject, IEngineControl
    {
        private const int IdleSpeed = 250;
        private const string RampingState = "ramping";

        private readonly ILog _log;
        private readonly IEventAggregator _eventAggregator;
        private readonly ManualResetEvent _emergencyStopEvent;
        private readonly ManualResetEvent _stopEvent;
        private readonly TouchSensor _touchSensor;
        private readonly MediumMotor _motor;
        private Thread _emergencyStopThread;
        private bool _disposed;

        public EngineControl
        (
            ILog log,
            IEventAggregator eventAggregator
        )
        {
            _log = log;
            _eventAggregator = eventAggregator;
            _stopEvent = new ManualResetEvent(false);
            _touchSensor = new TouchSensor(Inputs.Input1);

            _emergencyStopEvent = new ManualResetEvent(false);
            _motor = new MediumMotor(Outputs.OutputA) { RampUpSp = 0, RampDownSp = 0 };
        }

        public override object InitializeLifetimeService() => null;

        public string ServiceName => nameof(EngineControl);

        public void Prepare()
        {
            _log.Info("Prepare engine control...");
            _emergencyStopThread = new Thread(() =>
            {
                while (!_stopEvent.WaitOne(10))
                {
                    if (_touchSensor.State)
                    {
                        if (Speed > 0)
                        {
                            _log.Warn(@"/!\ Emergency stop");
                            _emergencyStopEvent.Set();
                            Stop(true);
                            _eventAggregator.GetEvent<EmergencyStop>().Publish();

                        }
                        else
                        {
                            Idle();
                        }
                        Console.Write(@"> ");
                    }
                }

                if (Speed > 0)
                {
                    Idle();
                    Stop();
                }
            });
            _emergencyStopThread.Start();
        }

        public void Unprepare()
        {
            _log.Info("Unprepare engine control...");
            if (_emergencyStopThread != null)
            {
                _stopEvent.Set();
                _emergencyStopThread.Join();
                _stopEvent.Reset();
                _emergencyStopThread = null;
            }
        }

        public int Speed
        {
            get => _motor.Speed;
            set
            {
                _motor.SpeedSp = value;
                _motor.RunForever();
            }
        }

        public int Duty
        {
            get => _motor.DutyCycle;
            set => _motor.DutyCycleSp = value;
        }

        public void Start()
        {
            _log.Info("Starting engine...");
            _motor.RunForever();
        }

        public void Stop(bool brake = false)
        {
            _log.Info("Stopping engine...");
            _motor.RampUpSp = 0;
            _motor.RampDownSp = 0;
            _motor.StopCommand = brake ? Motor.StopCommandBrake : Motor.StopCommandCoast;
            _motor.Stop();
            _motor.SpeedSp = 0;
        }

        public void Ramp(int speed, double time)
        {
            var currentSpeed = Speed;
            var speedDif = speed - currentSpeed;
            if (speedDif == 0)
                return;

            var timeMs = Convert.ToInt32(time * 1000);
            var rampSp = Convert.ToInt32((timeMs * 1560) / System.Math.Abs(speedDif));
            _motor.RampUpSp = rampSp;
            Thread.Sleep(100);
            _motor.RampDownSp = rampSp;
            Thread.Sleep(100);

            var start = DateTime.Now;
            _log.Info($"Starting ramp ({currentSpeed} => {speed} in {time:0.00} s)...");
            Speed = speed;

            while (!_emergencyStopEvent.WaitOne(10) && _motor.State.Any(s => s.EqualsNoCase(RampingState)))
            {
            }
            _emergencyStopEvent.Reset();

            _log.Info($"Done: {(DateTime.Now - start).TotalMilliseconds / 1000} s");
            _log.Info($"Speed: {Speed}");

            _motor.RampUpSp = 0;
            Thread.Sleep(100);
            _motor.RampDownSp = 0;
            Thread.Sleep(100);
        }

        public void Idle()
        {
            if (Speed <= 0)
            {
                _log.Info("Starting engine...");
                Ramp(400, 1);
                Thread.Sleep(1000);
            }
            _log.Info("Go to idle...");
            Ramp(IdleSpeed, 3);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = disposing;
            Unprepare();
            _emergencyStopEvent?.Dispose();
            _stopEvent?.Dispose();
            _touchSensor?.Dispose();
            _motor?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
