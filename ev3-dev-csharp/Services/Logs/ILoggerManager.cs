using log4net;

namespace EV3.Dev.Csharp.Services.Logs
{
	public interface ILoggerManager
	{
		ILog Logger { get; }
	}
}