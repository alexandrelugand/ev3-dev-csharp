using EV3.Dev.Csharp.Core.DesignPatterns;
using EV3.Dev.Csharp.Services.Logs;
using EV3.Dev.Csharp.Services.Sound;
using Unity;

namespace EV3.Dev.Csharp.Services
{
	public class Ev3Services : IEv3Services
	{
		private readonly UnityContainer _container;

		private Ev3Services()
		{
			_container = new UnityContainer();
			_container.RegisterType<ILoggerManager, LoggerManager>(TypeLifetime.Singleton);
			_container.RegisterType<ISoundManager, SoundManager>(TypeLifetime.Singleton);

			var loggerManager = GetService<ILoggerManager>();
			_container.RegisterInstance(loggerManager.Logger);
		}

		public static Ev3Services Instance => Singleton<Ev3Services>.Instance;

		public T GetService<T>() where T : class => _container.Resolve<T>();
	}
}
