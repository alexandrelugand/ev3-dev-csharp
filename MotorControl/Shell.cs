using DynamicExpresso;
using EV3.Dev.Csharp.Core.Helpers;
using log4net;
using MotorControl.Commands;
using System;

namespace MotorControl
{
    public class Shell
    {
        private readonly ILog _logger;
        private readonly Interpreter _interpreter;

        public Shell(ILog logger)
        {
            _logger = logger;
            _interpreter = new Interpreter();
            _interpreter.SetVariable("lm", new CommandsLargeMotor());
            _interpreter.SetVariable("mm", new CommandsMediumMotor());
        }

        public object Eval(string expression)
        {
            try
            {
                if (string.IsNullOrEmpty(expression))
                    return null;

                return _interpreter.Eval(expression);
            }
            catch (Exception ex)
            {
                _logger.Status(Status.KO, "Command failed: {0}", ex.Message);
                return null;
            }
        }
    }
}
