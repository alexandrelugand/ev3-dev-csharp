using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicExpresso;
using EV3.Dev.Csharp.Core.Helpers;
using log4net;
using MotorControl.Commands;

namespace MotorControl
{
    public class Shell
    {
	    private readonly ILog _logger;
	    private readonly Interpreter _interpreter;
	    private readonly CommandsLargeMotor _commandsLargeMotor;
	    private readonly CommandsMediumMotor _commandsMediumMotor;

	    public Shell(ILog logger)
	    {
		    _logger = logger;
		    _interpreter = new Interpreter();
			_commandsLargeMotor = new CommandsLargeMotor();
			_commandsMediumMotor = new CommandsMediumMotor();

			_interpreter.SetVariable("lm", _commandsLargeMotor);
			_interpreter.SetVariable("mm", _commandsMediumMotor);
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
