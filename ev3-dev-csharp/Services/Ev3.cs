using EV3.Dev.Csharp.Core.DesignPatterns;
using EV3.Dev.Csharp.Services.Logs;
using EV3.Dev.Csharp.Services.Remoting;
using EV3.Dev.Csharp.Services.Resources;
using EV3.Dev.Csharp.Services.Sound;
using Prism.Events;
using System;
using System.Collections.Generic;
using Unity;

namespace EV3.Dev.Csharp.Services
{
    public sealed class Ev3 : IEv3
    {
        private readonly IUnityContainer _container;
        private bool _disposed;

        private Ev3()
        {
            _container = new UnityContainer();
            _container.RegisterInstance<IEv3>(this);
            _container.RegisterType<ILoggerManager, LoggerManager>(TypeLifetime.Singleton);
            _container.RegisterType<IResourceManager, ResourceManager>(TypeLifetime.Singleton);
            _container.RegisterType<ISoundManager, SoundManager>(TypeLifetime.Singleton);
            _container.RegisterType<IEventAggregator, EventAggregator>(TypeLifetime.Singleton);
            _container.RegisterType<IRemoteServices, RemoteServices>(TypeLifetime.Singleton);
            _container.RegisterType<IRemoteController, RemoteController>(TypeLifetime.Singleton);

            var loggerManager = Resolve<ILoggerManager>();
            _container.RegisterInstance(loggerManager.Logger);
        }

        public static IEv3 Instance => Singleton<Ev3>.Instance;

        public void Init(Action<IUnityContainer> callback) => callback(_container);

        public IUnityContainer Container => _container;

        public T Resolve<T>(string name = null) where T : class
        {
            if (!string.IsNullOrEmpty(name))
                return _container.Resolve<T>(name);
            return _container.Resolve<T>();
        }

        public object Resolve(Type type, string name = null)
        {
            if (!string.IsNullOrEmpty(name))
                return _container.Resolve(type, name);
            return _container.Resolve(type);
        }

        public IEnumerable<T> ResolveAll<T>() where T : class => _container.ResolveAll<T>();

        public IEnumerable<object> ResolveAll(Type type) => _container.ResolveAll(type);

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = disposing;
                _container?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
