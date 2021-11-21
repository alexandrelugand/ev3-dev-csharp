using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Services;
using EV3.Dev.Csharp.Services.Remoting;
using Ev3System.Services.Engine;
using Ev3System.Services.Gearbox;
using log4net;
using McMaster.Extensions.CommandLineUtils;
using MotorControl.Commands;
using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity;

namespace MotorControl
{
    public class Program
    {
        private static ILog Log { get; set; }

        public static async Task<int> Main(string[] args)
        {
            try
            {
                using (var ev3 = Ev3.Instance)
                {
                    ev3.Init(c =>
                    {
                        var hostName = ConfigurationManager.AppSettings["HostName"];
                        var remoteServices = RemoteController.Connect(hostName);
                        if (remoteServices.AvailableServices.Any(s => s.EqualsNoCase(nameof(EngineControl))))
                        {
                            if (remoteServices.GetService(nameof(EngineControl)) is IEngineControl engineControl)
                                c.RegisterInstance(engineControl);
                        }

                        if (remoteServices.AvailableServices.Any(s => s.EqualsNoCase(nameof(GearboxControl))))
                        {
                            if (remoteServices.GetService(nameof(GearboxControl)) is IGearboxControl gearboxControl)
                                c.RegisterInstance(gearboxControl);
                        }
                    });
                    Log = ev3.Resolve<ILog>();
                    Log.Clear();

                    //Display version
                    var cmd = "--version";
                    await CommandLineApplication.ExecuteAsync<HelpCmd>(cmd);

                    do
                    {
                        if (args.Length > 0)
                            await CommandLineApplication.ExecuteAsync<HelpCmd>(args);
                        Console.Write(@"> ");

                        cmd = Console.ReadLine();
                        if (!string.IsNullOrEmpty(cmd))
                        {
                            var regex = new Regex(@"""(.*?)""");
                            foreach (var str in regex.Matches(cmd).Cast<Match>().Select(m => m.Value))
                            {
                                cmd = cmd.Replace(str, str.Replace(" ", "$_$"));
                            }

                            args = cmd.Split(' ').Select(c => c.Replace("$_$", " ")).ToArray();
                        }

                    } while (!cmd.EqualsNoCase("quit") && !cmd.EqualsNoCase("exit"));
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
            return 0;
        }
    }
}
