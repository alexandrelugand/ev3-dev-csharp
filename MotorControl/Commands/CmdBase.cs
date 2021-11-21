using System;
using System.Threading.Tasks;
using EV3.Dev.Csharp.Services;
using Ev3System.Services.Engine;
using Ev3System.Services.Gearbox;
using log4net;
using McMaster.Extensions.CommandLineUtils;

namespace MotorControl.Commands
{
    [HelpOption("--help")]
    public abstract class CmdBase
    {
        protected readonly IConsole Console;
        protected readonly IEv3 Ev3;
        protected readonly ILog Log;
        protected readonly IEngineControl EngineControl;
        protected readonly IGearboxControl GearboxControl;


        protected CmdBase(IConsole console)
        {
            Console = console;
            Ev3 = EV3.Dev.Csharp.Services.Ev3.Instance;
            Log = Ev3.Resolve<ILog>();
            EngineControl = Ev3.Resolve<IEngineControl>();
            GearboxControl = Ev3.Resolve<IGearboxControl>();
        }

        protected virtual Task<int> OnExecute(CommandLineApplication app) => Task.FromResult(0);

        protected void OnException(Exception ex) => OutputError(ex, $"{GetType().Name}");

        protected void Output(string data)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine($" {data}");
            Console.ResetColor();
            Log.Debug(data);
        }

        protected void OutputError(string data)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Error.WriteLine(data);
            Console.ResetColor();
            Log.Debug(data);
        }

        protected void OutputError(Exception ex, string data)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Error.WriteLine($"{data}. Exception: {ex}");
            Console.ResetColor();
            Log.Debug(data, ex);
        }
    }
}
