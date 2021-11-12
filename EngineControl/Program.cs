using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Services;
using EV3.Dev.Csharp.Services.Remoting;
using EV3.Dev.Csharp.Services.Sound;
using Ev3System.Services.Engine;
using log4net;
using System;
using System.Configuration;
using System.Reflection;
using Unity;

namespace EngineControl
{
    public class Program
    {
        private static ILog Log { get; set; }

        public static void Main()
        {

            try
            {
                using (var ev3 = Ev3.Instance)
                {
                    ev3.Init(c =>
                    {
                        c.RegisterType<IEngineControl, Ev3System.Services.Engine.EngineControl>(TypeLifetime.Singleton);
                    });
                    Log = ev3.Resolve<ILog>();
                    var soundManager = ev3.Resolve<ISoundManager>();
                    Log.Clear();

                    //Display version
                    var assemblyFileVersionAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>();
                    var assemblyCopyrightAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>();
                    Log.Info($"Engine Control ({assemblyFileVersionAttribute.Version}) {assemblyCopyrightAttribute.Copyright}");

                    var remoteServices = ev3.Resolve<IRemoteServices>();
                    var remoteController = ev3.Resolve<IRemoteController>();
                    if (remoteController.StartRemotingServer(remoteServices, NetworkInterfaceManager.GetIpAddress(ConfigurationManager.AppSettings["InterfaceName"])))
                    {
                        if (remoteServices.GetService(nameof(EngineControl)) is IEngineControl engineControl)
                        {
                            engineControl.Prepare();

                            soundManager.Load();
                            soundManager.PlaySound("ready");

                            string cmd;

                            do
                            {
                                Console.Write(@"> ");
                                cmd = Console.ReadLine();
                            } while (!cmd.EqualsNoCase("quit") && !cmd.EqualsNoCase("exit"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Log != null)
                {
                    Log.Status(Status.KO, "Unexpected error");
                    Log.Error("Stack trace", ex);
                    Console.ReadKey();
                }
            }
        }
    }
}
