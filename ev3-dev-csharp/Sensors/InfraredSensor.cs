using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Sensors.Ir;

namespace EV3.Dev.Csharp.Sensors
{
    /// <summary> 
    /// LEGO EV3 infrared sensor.
    /// </summary>
    public class InfraredSensor : Sensor
    {
        public InfraredSensor(string port)
            : base(port, DeviceTypes.IrSensor, new[] { Drivers.LegoEv3Ir })
        {

        }

        /// <summary> 
        /// Proximity
        /// </summary>
        public const string ModeIrProx = "IR-PROX";

        /// <summary> 
        /// IR Seeker
        /// </summary>
        public const string ModeIrSeek = "IR-SEEK";

        /// <summary> 
        /// IR Remote Control
        /// </summary>
        public const string ModeIrRemote = "IR-REMOTE";

        /// <summary> 
        /// IR Remote Control. State of the buttons is coded in binary
        /// </summary>
        public const string ModeIrRemA = "IR-REM-A";

        /// <summary> 
        /// Calibration ???
        /// </summary>
        public const string ModeIrCal = "IR-CAL";

        /// <summary> 
        /// Proximity
        /// </summary>        
        public void SetIrProx() { Mode = ModeIrProx; }
        public bool IsIrProx() { return Mode == ModeIrProx; }

        /// <summary> 
        /// IR Seeker
        /// </summary>        
        public void SetIrSeek() { Mode = ModeIrSeek; }
        public bool IsIrSeek() { return Mode == ModeIrSeek; }

        /// <summary> 
        /// IR Remote Control
        /// </summary>        
        public void SetIrRemote() { Mode = ModeIrRemote; }
        public bool IsIrRemote() { return Mode == ModeIrRemote; }

        /// <summary> 
        /// IR Remote Control. State of the buttons is coded in binary
        /// </summary>        
        public void SetIrRemA() { Mode = ModeIrRemA; }
        public bool IsIrRemA() { return Mode == ModeIrRemA; }

        /// <summary> 
        /// Calibration ???
        /// </summary>        
        public void SetIrCal() { Mode = ModeIrCal; }
        public bool IsIrCal() { return Mode == ModeIrCal; }


        public byte Proximity => (byte)(IsIrProx() ? GetInt() : 0);

        public IrSeeker IrSeeker => IsIrSeek() ? new IrSeeker
        {
            Ch1Heading = (sbyte)GetInt(),
            Ch1Distance = (sbyte)GetInt(1),
            Ch2Heading = (sbyte)GetInt(2),
            Ch2Distance = (sbyte)GetInt(3),
            Ch3Heading = (sbyte)GetInt(4),
            Ch3Distance = (sbyte)GetInt(5),
            Ch4Heading = (sbyte)GetInt(6),
            Ch4Distance = (sbyte)GetInt(7),
        } : default(IrSeeker);

        public IrRemote IrRemote => IsIrRemote() ? new IrRemote
        {
            Ch1 = (byte)GetInt(),
            Ch2 = (byte)GetInt(1),
            Ch3 = (byte)GetInt(2),
            Ch4 = (byte)GetInt(3)
        } : default(IrRemote);

        public ushort IrRemA => (ushort)(IsIrRemA() ? GetInt() : 0);

        public IrCal IrCal => IsIrCal() ? new IrCal
        {
            Value0 = (ushort)GetInt(),
            Value1 = (ushort)GetInt(1)
        } : default(IrCal);
    }
}