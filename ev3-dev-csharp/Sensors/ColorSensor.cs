using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Sensors.Color;

namespace EV3.Dev.Csharp.Sensors
{
    /// <summary> 
    /// LEGO EV3 color sensor.
    /// </summary>
    public class ColorSensor : Sensor
    {
        public ColorSensor(string port)
            : base(port, DeviceTypes.ColorSensor, new[] { Drivers.LegoEv3Color })
        {
        }

        /// <summary> 
        /// Reflected light. Red LED on.
        /// </summary>
        public const string ModeColReflect = "COL-REFLECT";

        /// <summary> 
        /// Ambient light. Red LEDs off.
        /// </summary>
        public const string ModeColAmbient = "COL-AMBIENT";

        /// <summary> 
        /// Color. All LEDs rapidly cycling, appears white.
        /// </summary>
        public const string ModeColColor = "COL-COLOR";

        /// <summary> 
        /// Raw reflected. Red LED on
        /// </summary>
        public const string ModeRefRaw = "REF-RAW";

        /// <summary> 
        /// Raw Color Components. All LEDs rapidly cycling, appears white.
        /// </summary>
        public const string ModeRgbRaw = "RGB-RAW";

        public void SetColReflect() { Mode = ModeColReflect; }
        public bool IsColReflect() { return Mode == ModeColReflect; }

        public void SetColAmbient() { Mode = ModeColAmbient; }
        public bool IsColAmbient() { return Mode == ModeColAmbient; }

        public void SetColColor() { Mode = ModeColColor; }
        public bool IsColColor() { return Mode == ModeColColor; }

        public void SetRefRaw() { Mode = ModeRefRaw; }
        public bool IsRefRaw() { return Mode == ModeRefRaw; }

        public void SetRgbRaw() { Mode = ModeRgbRaw; }
        public bool IsRgbRaw() { return Mode == ModeRgbRaw; }

        public byte Reflect => (byte)(IsColReflect() ? GetInt() : 0);
        public byte Ambient => (byte)(IsColAmbient() ? GetInt() : 0);
        public Colors Color => (Colors)(IsColColor() ? GetInt() : 0);
        public RefRaw RefRaw => IsRefRaw() ? new RefRaw { Value0 = (short)GetInt(), Value1 = (short)GetInt(1) } : default(RefRaw);
        public RgbRaw RgbRaw => IsRgbRaw() ? new RgbRaw { Value0 = (short)GetInt(), Value1 = (short)GetInt(1), Value2 = (short)GetInt(2) } : default(RgbRaw);

    }
}