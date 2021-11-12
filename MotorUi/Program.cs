using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Services;
using EV3.Dev.Csharp.Services.Remoting;
using Ev3System.Services.Engine;
using log4net;
using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using Unity;

namespace MotorUi
{
    public static class Program
    {
        private static ILog Log { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                using (var ev3 = Ev3.Instance)
                {
                    IEngineControl engineControl = null;
                    ev3.Init(c =>
                    {
                        var hostName = ConfigurationManager.AppSettings["HostName"];
                        var remoteServices = RemoteController.Connect(hostName);
                        if (remoteServices.AvailableServices.Any(s => s.EqualsNoCase(nameof(EngineControl))))
                        {
                            engineControl = remoteServices.GetService(nameof(EngineControl)) as IEngineControl;
                            c.RegisterInstance(engineControl);
                        }
                    });
                    Log = ev3.Resolve<ILog>();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm(engineControl));
                }
            }
            catch (Exception ex)
            {
                Log?.Error("Stack trace", ex);
            }
        }
    }
}
