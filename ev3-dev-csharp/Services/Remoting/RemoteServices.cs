using EV3.Dev.Csharp.Core.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace EV3.Dev.Csharp.Services.Remoting
{
    public class RemoteServices : MarshalByRefObject, IRemoteServices, IRemoteService
    {
        private readonly ILog _log;
        private bool _disposed;
        private readonly List<IRemoteService> _remoteServices;

        public RemoteServices(IEv3 ev3, ILog log)
        {
            _log = log;
            _remoteServices = new List<IRemoteService>();

            var remoteServiceTypes = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && t.IsPublic && typeof(IRemoteService).IsAssignableFrom(t) && !t.Name.EqualsNoCase(nameof(RemoteServices)))
                .ToList();

            foreach (var remoteServiceType in remoteServiceTypes)
            {
                ev3.Container.RegisterType(typeof(IRemoteService), remoteServiceType, remoteServiceType.Name, TypeLifetime.Singleton);
                // ReSharper disable once SuspiciousTypeConversion.Global
                if (ev3.Resolve<IRemoteService>(remoteServiceType.Name) is IRemoteService remoteService)
                    _remoteServices.Add(remoteService);
            }


        }

        public string Version { get; set; }
        public List<string> AvailableServices => _remoteServices.Select(r => r.ServiceName).ToList();

        public string ServiceName => nameof(RemoteServices);

        public IRemoteService GetService(string serviceName)
        {
            try
            {
                _log.Info($"Requesting {serviceName} service...");
                return _remoteServices.FirstOrDefault(r => r.ServiceName.EqualsNoCase(serviceName));
            }
            catch (Exception ex)
            {
                _log.Error($"Failed to get {serviceName} service", ex);
            }

            return null;
        }

        public void ReleaseService(IRemoteService service)
        {
            _log.Info($"Release {service.ServiceName} service...");
            if (service is IDisposable disposable)
                disposable.Dispose();
        }

        public override object InitializeLifetimeService() => null;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _log.Info("Disposing remote services...");
            _disposed = disposing;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}