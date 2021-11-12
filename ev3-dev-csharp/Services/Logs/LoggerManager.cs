using log4net;
using log4net.Config;
using log4net.Repository;
using System.IO;
using System.Reflection;
// ReSharper disable AssignNullToNotNullAttribute

namespace EV3.Dev.Csharp.Services.Logs
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly ILoggerRepository LoggerRepository = LogManager.CreateRepository("EV3");

        public LoggerManager()
        {
            try
            {
                XmlConfigurator.Configure(LoggerRepository, new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log4net.config")));
                var h = (log4net.Repository.Hierarchy.Hierarchy)LoggerRepository;
                foreach (var a in h.Root.Appenders)
                {
                    if (a is log4net.Appender.FileAppender fileAppender)
                    {
                        var logDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "logs");
                        if (!Directory.Exists(logDir))
                            Directory.CreateDirectory(logDir);
                        fileAppender.File = Path.Combine(logDir, Path.GetFileName(fileAppender.File));
                        fileAppender.ActivateOptions();
                    }
                }
                Logger = LogManager.GetLogger("EV3", MethodBase.GetCurrentMethod().DeclaringType);
            }
            catch
            {
                Logger = null;
            }
        }

        public ILog Logger { get; set; }
    }
}
