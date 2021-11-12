using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace EngineControl.Commands
{
    [Command(Name = "Speed", Description = "Gets or sets motor speed")]
    public class SpeedCmd : CmdBase
    {

        [Option(CommandOptionType.SingleOrNoValue, ShortName = "v", LongName = "value", Description = "Motor speed", ShowInHelpText = true)]
        public int? Speed { get; set; }

        public SpeedCmd(IConsole console)
            : base(console)
        {
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            try
            {
                if (Speed == null)
                {
                    Output($"Speed: {EngineControl.Speed}");
                    return Task.FromResult(1);
                }

                EngineControl.Speed = Speed.Value;
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