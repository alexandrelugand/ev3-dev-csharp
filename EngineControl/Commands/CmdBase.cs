using EV3.Dev.Csharp.Services;
using Ev3System.Services.Engine;
using log4net;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Threading.Tasks;

namespace EngineControl.Commands
{
    [HelpOption("--help")]
    public abstract class CmdBase
    {
        protected readonly IConsole Console;
        protected readonly IEv3 Ev3;
        protected readonly ILog Log;
        protected readonly IEngineControl EngineControl;


        protected CmdBase(IConsole console)
        {
            Console = console;
            Ev3 = EV3.Dev.Csharp.Services.Ev3.Instance;
            Log = Ev3.Resolve<ILog>();
            EngineControl = Ev3.Resolve<IEngineControl>();
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
