using EV3.Dev.Csharp.Constants;

namespace EV3.Dev.Csharp.Sensors
{
    /// <summary> 
    /// Touch Sensor
    /// </summary>
    public class TouchSensor : Sensor
    {
        public TouchSensor(string port)
            : base(port, DeviceTypes.TouchSensor, new[] { Drivers.LegoEv3Touch, Drivers.LegoNxtTouch })
        {
        }


        public bool State => GetInt() != 0;
    }
}