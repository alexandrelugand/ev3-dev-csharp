using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace EngineControl.Commands
{
    [Command(Name = "Idle", Description = "Start or bring back motor to Idle speed")]
    public class IdleCmd : CmdBase
    {

        public IdleCmd(IConsole console)
            : base(console)
        {

        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            try
            {
                EngineControl.Idle();
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
