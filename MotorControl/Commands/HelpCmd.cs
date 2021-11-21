using McMaster.Extensions.CommandLineUtils;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MotorControl.Commands
{

    [Command(Name = " ", OptionsComparison = StringComparison.InvariantCultureIgnoreCase)]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(IdleCmd),
        typeof(StopCmd),
        typeof(SpeedCmd),
        typeof(RampCmd),
        typeof(GearCmd)
    )]
    public class HelpCmd : CmdBase
    {
        private static readonly AssemblyFileVersionAttribute AssemblyFileVersionAttribute = typeof(HelpCmd).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
        private static readonly AssemblyCopyrightAttribute AssemblyCopyrightAttribute = typeof(HelpCmd).Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();

        public HelpCmd(IConsole console)
            : base(console)
        {
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return Task.FromResult(0);
        }

        private static string GetVersion() => $"Motor CLI ({AssemblyFileVersionAttribute.Version}) {AssemblyCopyrightAttribute.Copyright}";
    }
}
