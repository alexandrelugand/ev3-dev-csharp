using EV3.Dev.Csharp.Constants;

namespace EV3.Dev.Csharp.Sensors
{
    /// <summary> 
    /// LEGO EV3 ultrasonic sensor.
    /// </summary>
    public class UltrasonicSensor : Sensor
    {
        private const string UsDistCm = "US-DIST-CM";
        private const string UsDistInch = "US-DIST-IN";
        private const string UsListen = "US-LISTEN";
        private const string UsSingleCm = "US-SI-CM";
        private const string UsSingleInch = "US-SI-IN";

        public UltrasonicSensor(string port)
            : base(port, DeviceTypes.UltraSonicSensor, new[] { Drivers.LegoEv3Us, Drivers.LegoNxtUs })
        {
            DistCmMode = true;
        }

        /// <summary> 
        /// Continuous measurement in centimeters (default mode).
        /// LEDs: On, steady
        /// </summary>

        public bool DistCmMode
        {
            get => Mode == UsDistCm;
            // ReSharper disable once ValueParameterNotUsed
            set => Mode = UsDistCm;
        }

        /// <summary> 
        /// Continuous measurement in inches.
        /// LEDs: On, steady
        /// </summary>
        public bool DistInchMode
        {
            get => Mode == UsDistInch;
            set => Mode = value ? UsDistInch : UsDistCm;
        }

        /// <summary> 
        /// Listen.  LEDs: On, blinking
        /// </summary>
        public bool ListenMode
        {
            get => Mode == UsListen;
            set => Mode = value ? UsListen : UsDistCm;
        }

        /// <summary> 
        /// Single measurement in centimeters.
        /// LEDs: On momentarily when mode is set, then off
        /// </summary>
        public bool SingleCmModel
        {
            get => Mode == UsSingleCm;
            set => Mode = value ? UsSingleCm : UsDistCm;
        }

        /// <summary> 
        /// Single measurement in inches.
        /// LEDs: On momentarily when mode is set, then off
        /// </summary>
        public bool SingleInchModel
        {
            get => Mode == UsSingleInch;
            set => Mode = value ? UsSingleInch : UsDistCm;
        }

        /// <summary>
        /// Continuous measurement in cm (0-2550).
        /// </summary>
        public short DistanceCm => (short)GetInt();

        /// <summary>
        /// Continuous measurement in inches (0-1003).
        /// </summary>
        public short DistanceInch => (short)GetInt();

        /// <summary>
        /// Listen activated.
        /// </summary>
        public bool Listen => GetInt() != 0;

        /// <summary> 
        /// Single measurement in centimeters (0-2550).
        /// </summary>
        public short SingleCm => (short)GetInt();

        /// <summary> 
        /// Single measurement in inches (0-1003).
        /// </summary>
        public short SingleInch => (short)GetInt();
    }
}