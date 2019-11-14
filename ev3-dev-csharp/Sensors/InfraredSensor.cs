using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Sensors.Ir;


namespace EV3.Dev.Csharp.Sensors
{
    /// <summary> 
    /// LEGO EV3 infrared sensor.
    /// </summary>
    public class InfraredSensor : Sensor
    {
        private const string IrProx = "IR-PROX";
        private const string IrSeek = "IR-SEEK";
        private const string IrRemote = "IR-REMOTE";
        private const string IrRemA = "IR-REM-A";
        private const string IrCal = "IR-CAL";

        public InfraredSensor(string port)
            : base(port, DeviceTypes.IrSensor, new[] { Drivers.LegoEv3Ir })
        {
            ProximityMode = true;
        }

        /// <summary> 
        /// Proximity (default mode)
        /// </summary>        
        public bool ProximityMode
        {
            get => Mode == IrProx;
            // ReSharper disable once ValueParameterNotUsed
            set => Mode = IrProx;
        }


        /// <summary> 
        /// IR Seeker
        /// </summary>        
        public bool SeekMode
        {
            get => Mode == IrSeek;
            set => Mode = value ? IrSeek : IrProx;
        }

        /// <summary> 
        /// IR Remote Control
        /// </summary>        
        public bool RemoteMode
        {
            get => Mode == IrRemote;
            set => Mode = value ? IrRemote : IrProx;
        }

        /// <summary> 
        /// IR Remote Control. State of the buttons is coded in binary
        /// </summary>        
        public bool RemoteAlternativeMode
        {
            get => Mode == IrRemA;
            set => Mode = value ? IrRemA : IrProx;
        }

        /// <summary> 
        /// Calibration ???
        /// </summary>        
        public bool CalibrationMode
        {
            get => Mode == IrCal;
            set => Mode = value ? IrCal : IrProx;
        }

        /// <summary>
        /// Distance (%): 0 to 100
        /// 100% is approximately 70cm/27in.
        /// </summary>

        public byte Proximity => (byte)(ProximityMode ? GetInt() : 0);

        /// <summary>
        /// IR Seeker (%):
        /// Channel 1 Heading (-25 to 25). When looking in the same direction as the sensor, -25 is far left and +25 is far right.
        /// Channel 1 Distance (-128 and 0 to 100). 100% is approximately 200cm/78in. The absence of a beacon on a channel can be detected when distance == -128 (and heading == 0).
        /// Channel 2 Heading (-25 to 25). When looking in the same direction as the sensor, -25 is far left and +25 is far right.
        /// Channel 2 Distance (-128 and 0 to 100). 100% is approximately 200cm/78in. The absence of a beacon on a channel can be detected when distance == -128 (and heading == 0).
        /// Channel 3 Heading (-25 to 25). When looking in the same direction as the sensor, -25 is far left and +25 is far right.
        /// Channel 3 Distance (-128 and 0 to 100). 100% is approximately 200cm/78in. The absence of a beacon on a channel can be detected when distance == -128 (and heading == 0).
        /// Channel 4 Heading (-25 to 25). When looking in the same direction as the sensor, -25 is far left and +25 is far right.
        /// Channel 4 Distance (-128 and 0 to 100). 100% is approximately 200cm/78in. The absence of a beacon on a channel can be detected when distance == -128 (and heading == 0).
        /// </summary>
        public IrSeeker Seeker => SeekMode ? new IrSeeker
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


        /// <summary>
        /// IR Remote for channels 1 to 4 (state):
        /// 0	none
        /// 1	red up
        /// 2	red down
        /// 3	blue up
        /// 4	blue down
        /// 5	red up and blue up
        /// 6	red up and blue down
        /// 7	red down and blue up
        /// 8	red down and blue down
        /// 9	beacon mode on
        /// 10	red up and red down
        /// 11	blue up and blue down
        /// </summary>
        public IrRemote Remote => RemoteMode ? new IrRemote
        {
            Ch1 = (byte)GetInt(),
            Ch2 = (byte)GetInt(1),
            Ch3 = (byte)GetInt(2),
            Ch4 = (byte)GetInt(3)
        } : default(IrRemote);

        /// <summary>
        /// IR Remote alternative (mask of bits):
        /// The most significant byte is always 0x01.
        /// In the least significant byte, the 4 most significant bits represent each button.
        /// Bit 7 is the blue down button, bit 6 is the blue up button, bit 5 is the red down button, bit 4 is the red up button.
        /// Beware that when no buttons are pressed, bit 7 is set (value == 384).
        /// </summary>
        public ushort RemoteAlternative => (ushort)(RemoteAlternativeMode ? GetInt() : 0);

        /// <summary>
        /// Calibration:
        /// Value0 (0 to 1023)
        /// Value1 (0 to 1023)
        /// </summary>
        public IrCal Calibration => CalibrationMode ? new IrCal
        {
            Value0 = (ushort)GetInt(),
            Value1 = (ushort)GetInt(1)
        } : default(IrCal);
    }
}