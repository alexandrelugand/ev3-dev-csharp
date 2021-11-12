using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Sensors.Color;
using EV3.Dev.Csharp.Services;
using EV3.Dev.Csharp.Services.Sound;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using log4net;

namespace HeadControl
{
    public class Program
    {
        private static ILog Log { get; set; }

        public static void Main(string[] args)
        {
            try
            {
                var ev3Services = Ev3.Instance;
                Log = ev3Services.Resolve<ILog>();
                Log.Clear();

                var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                var version = fvi.FileVersion;
                Log.Info($"###### EV3 Head Control ({version}) ######");

                var soundManager = ev3Services.Resolve<ISoundManager>();
                soundManager.Volume = 10;
                var headControl = new HeadControl(Inputs.Input4, Inputs.Input1, Inputs.Input3, Outputs.OutputB, Log);
                soundManager.PlaySound("connect");
                headControl.Calibrate();
                soundManager.PlaySound("ready");

                while (true)
                {
                    Thread.Sleep(100);
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Escape)
                            break;
                    }

                    headControl.Update();

                    if (headControl.Distance < 50)
                    {
                        soundManager.PlaySound("detected");
                        if (headControl.OutOnLeft())
                        {
                            soundManager.PlaySound("turn");
                            soundManager.PlaySound("left");
                        }
                        else if (headControl.OutOnRight())
                        {
                            soundManager.PlaySound("turn");
                            soundManager.PlaySound("right");
                        }
                        else
                        {
                            soundManager.PlaySound("Backwards");
                        }
                    }

                    if (headControl.TouchPressed)
                        soundManager.PlaySound("okay");

                    if (headControl.Color != Colors.None)
                        soundManager.PlaySound(headControl.Color.ToString());
                }
            }
            catch (Exception ex)
            {
                if (Log != null)
                {
                    Log.Status(Status.KO, "Unexpected error");
                    Console.ReadKey();

                    Log.Error("Stack trace", ex);
                    Console.ReadKey();
                }
            }
        }
    }
}
