using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace EngineControl.Commands
{
    [Command(Name = "Ramp", Description = "Starts a ramp to reach speed with duration's defined")]
    public class RampCmd : CmdBase
    {

        [Option(CommandOptionType.SingleOrNoValue, ShortName = "s", LongName = "speed", Description = "Motor speed", ShowInHelpText = true)]
        public int Speed { get; set; }

        [Option(CommandOptionType.SingleOrNoValue, ShortName = "d", LongName = "duration", Description = "Duration in second", ShowInHelpText = true)]
        public double Duration { get; set; }

        public RampCmd(IConsole console)
            : base(console)
        {
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            try
            {
                if (Speed == 0)
                    Speed = Prompt.GetInt("Speed:");

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Duration == 0)
                    Duration = Convert.ToDouble(Prompt.GetString("Duration:"));

                EngineControl.Ramp(Speed, Duration);
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