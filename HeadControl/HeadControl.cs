using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Motors;
using EV3.Dev.Csharp.Sensors;
using EV3.Dev.Csharp.Sensors.Color;
using log4net;
using System.Threading;

namespace HeadControl
{
    public class HeadControl
    {
        private readonly ILog _logger;
        private readonly InfraredSensor _ir;
        private readonly Motor _motor;
        private readonly TouchSensor _touch;
        private readonly ColorSensor _color;

        private int _speed = 75;
        private int _couple = 5;

        private const int SecurityMarge = 10;
        private const int SafetyDistance = 80;

        public HeadControl(string irPort, string touchPort, string colorPort, string motorPort, ILog logger)
        {
            _logger = logger;
            _ir = new InfraredSensor(irPort);
            _ir.SetIrProx();

            _touch = new TouchSensor(touchPort);

            _color = new ColorSensor(colorPort);
            _color.SetColColor();

            _motor = new MediumMotor(motorPort)
            {
                SpeedSp = 0,
                StopCommand = Motor.StopCommandCoast
            };
            Stop();
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
            while (!_motor.Position.IsWithin(pos - SecurityMarge, pos + SecurityMarge))
                Thread.Sleep(50);
        }

        public void TurnLeftMax()
        {
            _motor.SpeedSp = _speed;
            _motor.DutyCycleSp = _couple;
            _motor.PositionSp = LeftStopPosition;
            _motor.RunToAbsPos();
            while (!_motor.Position.IsWithin(LeftStopPosition - SecurityMarge, LeftStopPosition + SecurityMarge))
                Thread.Sleep(50);
        }

        public void TurnLeftToMid()
        {
            _motor.SpeedSp = _speed;
            _motor.DutyCycleSp = _couple;
            _motor.PositionSp = MidStopPosition;
            _motor.RunToAbsPos();
            while (!_motor.Position.IsWithin(MidStopPosition - SecurityMarge, MidStopPosition + SecurityMarge))
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

        public void TurnRightMax()
        {
            _motor.SpeedSp = -_speed;
            _motor.DutyCycleSp = -_couple;
            _motor.PositionSp = RightStopPosition;
            _motor.RunToAbsPos();
            while (!_motor.Position.IsWithin(RightStopPosition - SecurityMarge, RightStopPosition + SecurityMarge))
                Thread.Sleep(50);
        }

        public void TurnRightToMid()
        {
            _motor.SpeedSp = -_speed;
            _motor.DutyCycleSp = -_couple;
            _motor.PositionSp = MidStopPosition;
            _motor.RunToAbsPos();
            while (!_motor.Position.IsWithin(MidStopPosition - SecurityMarge, MidStopPosition + SecurityMarge))
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

            MidStopPosition = (LeftStopPosition + RightStopPosition) / 2;
            _logger.Debug($"Mid stop position: {MidStopPosition}");

            _logger.Debug("Returns to mid position...");
            TurnLeftToPos(MidStopPosition);
            Stop();
            Thread.Sleep(1000);

            _logger.Debug("Increase speed...");
            _speed *= 2;
            TurnLeftToPos(LeftStopPosition);
            Stop();
            TurnRightToPos(RightStopPosition);
            Stop();
            TurnLeftToPos(MidStopPosition);
            Stop();

            _logger.Status(Status.OK, "Calibration ended");
        }

        public int Distance { get; private set; }
        public bool TouchPressed { get; private set; }
        public Colors Color { get; private set; }

        public void Update()
        {
            Distance = _ir.Proximity;
            TouchPressed = _touch.State;
            Color = _color.Color;
        }

        public bool OutOnLeft()
        {
            TurnLeftMax();
            var dist = _ir.GetInt();
            TurnRightToMid();
            return dist > SafetyDistance;
        }

        public bool OutOnRight()
        {
            TurnRightMax();
            var dist = _ir.GetInt();
            TurnLeftToMid();
            return dist > SafetyDistance;
        }
    }
}
