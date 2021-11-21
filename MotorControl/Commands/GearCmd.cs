﻿using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace MotorControl.Commands
{
    [Command(Name = "Gear", Description = "Shift up and down gearbox")]
    public class GearCmd : CmdBase
    {

        [Option(CommandOptionType.NoValue, ShortName = "u", LongName = "up", Description = "Gear up", ShowInHelpText = true)]
        public bool Up { get; set; }

        [Option(CommandOptionType.SingleOrNoValue, ShortName = "d", LongName = "down", Description = "Gear down", ShowInHelpText = true)]
        public bool Down { get; set; }

        public GearCmd(IConsole console)
            : base(console)
        {
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            try
            {
                if (Up)
                    GearboxControl.GearUp();

                if (Down)
                    GearboxControl.GearDown();

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