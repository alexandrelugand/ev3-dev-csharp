using McMaster.Extensions.CommandLineUtils;
using System;
using System.Threading.Tasks;

namespace MotorControl.Commands
{
    [Command(Name = "Stop", Description = "Stop motor")]
    public class StopCmd : CmdBase
    {

        public StopCmd(IConsole console)
            : base(console)
        {

        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            try
            {
                EngineControl.Stop();
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                OnException(ex);
                return Task.FromResult(2);
            }
        }
    }
}