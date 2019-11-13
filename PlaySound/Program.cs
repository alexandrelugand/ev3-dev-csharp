using EV3.Dev.Csharp.Constants;
using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Sensors;
using EV3.Dev.Csharp.Services;
using EV3.Dev.Csharp.Services.Sound;
using log4net;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace PlaySound
{
    public class Program
    {
        private static ILog Logger { get; set; }

        public static void Main(string[] args)
        {
            try
            {
                var ev3Services = Ev3Services.Instance;
                Logger = ev3Services.GetService<ILog>();
                var soundManager = ev3Services.GetService<ISoundManager>();
                Logger.Clear();

                var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                var version = fvi.FileVersion;
                Logger.Info($"###### EV3 Play sound ({version}) ######");

                var ir = new InfraredSensor(Inputs.Input4);
                ir.SetIrRemote();

                while (true)
                {
                    Thread.Sleep(100);
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Escape)
                            break;
                    }

                    switch (ir.GetInt())
                    {
                        case 1: // red up
                            soundManager.PlaySoundAsync("sound1");
                            break;
                        case 3: // blue up
                            soundManager.PlaySoundAsync("sound2");
                            break;
                        case 2:// red down
                            soundManager.PlaySoundAsync("sound3");
                            break;
                        case 4:// blue down
                            soundManager.PlaySoundAsync("sound4");
                            break;
                        case 9:
                            {
                                soundManager.InterruptSound();
                                break;
                            }
                    }
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
