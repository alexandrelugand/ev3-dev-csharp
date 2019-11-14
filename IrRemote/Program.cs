using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Sensors;
using EV3.Dev.Csharp.Services;
using EV3.Dev.Csharp.Services.Sound;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using log4net;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace IrRemote
{
    public class Program
    {
        private static ILog Logger { get; set; }

        public static void Main(string[] args)
        {
            try
            {
                var ev3Services = Ev3.Instance;
                Logger = ev3Services.Resolve<ILog>();
                var soundManager = ev3Services.Resolve<ISoundManager>();
                Logger.Clear();

                var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                var version = fvi.FileVersion;
                Logger.Info($"###### EV3 Infrared Remote ({version}) ######");

                var ir = new InfraredSensor(Inputs.Input4) { RemoteMode = true };
                var driveService = new DriveService(Outputs.OutputD, Outputs.OutputA, Logger);

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

                    var driveState = new DriveState(ir.Remote.Ch1);
                    driveService.Drive(driveState);
                }
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.Status(Status.KO, "Unexpected error");
                    Console.ReadKey();

                    Logger.Error("Stack trace", ex);
                    Console.ReadKey();
                }
            }
        }
    }
}
