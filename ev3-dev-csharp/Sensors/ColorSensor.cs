using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Sensors.Color;

namespace EV3.Dev.Csharp.Sensors
{
    /// <summary> 
    /// LEGO EV3 color sensor.
    /// </summary>
    public class ColorSensor : Sensor
    {
        private const string ColReflect = "COL-REFLECT";
        private const string ColAmbient = "COL-AMBIENT";
        private const string ColColor = "COL-COLOR";
        private const string ColRefRaw = "REF-RAW";
        private const string ColRgbRaw = "RGB-RAW";

        public ColorSensor(string port)
            : base(port, DeviceTypes.ColorSensor, new[] { Drivers.LegoEv3Color })
        {
            ReflectedMode = true;
        }


        /// <summary> 
        /// Reflected light. Red LED on (default mode).
        /// </summary>
        public bool ReflectedMode
        {
            get => Mode == ColReflect;
            // ReSharper disable once ValueParameterNotUsed
            set => Mode = ColReflect;
        }

        /// <summary> 
        /// Ambient light. Red LEDs off.
        /// </summary>
        public bool AmbientMode
        {
            get => Mode == ColAmbient;
            set => Mode = value ? ColAmbient : ColReflect;
        }

        /// <summary> 
        /// Color. All LEDs rapidly cycling, appears white.
        /// </summary>
        public bool ColorMode
        {
            get => Mode == ColColor;
            set => Mode = value ? ColColor : ColReflect;
        }

        /// <summary> 
        /// Raw reflected. Red LED on
        /// </summary>
        public bool RawReflectedMode
        {
            get => Mode == ColRefRaw;
            set => Mode = value ? ColRefRaw : ColReflect;
        }

        /// <summary> 
        /// Raw Color Components. All LEDs rapidly cycling, appears white.
        /// </summary>
        public bool RawColorMode
        {
            get => Mode == ColRgbRaw;
            set => Mode = value ? ColRgbRaw : ColReflect;
        }

        /// <summary>
        /// Reflected light intensity (0 to 100%)
        /// </summary>
        public byte Reflect => (byte)(ReflectedMode ? GetInt() : 0);

        /// <summary>
        /// Ambient light intensity (0 to 100%)
        /// </summary>
        public byte Ambient => (byte)(AmbientMode ? GetInt() : 0);

        /// <summary>
        /// Detected color
        /// </summary>
        public Colors Color => (Colors)(ColorMode ? GetInt() : 0);

        /// <summary>
        /// Raw Reflected:
        /// Value0 (0 to 1020)
        /// Value1 (0 to 1020)
        /// </summary>
        public RefRaw RawReflected => RawReflectedMode ? new RefRaw { Value0 = (short)GetInt(), Value1 = (short)GetInt(1) } : default(RefRaw);

        /// <summary>
        /// Raw Color Components :
        /// Value0 (0 to 1020)
        /// Value1 (0 to 1020)
        /// Value2 (0 to 1020)
        /// </summary>
        public RgbRaw RawColorRaw => RawColorMode ? new RgbRaw { Value0 = (short)GetInt(), Value1 = (short)GetInt(1), Value2 = (short)GetInt(2) } : default(RgbRaw);

    }
}