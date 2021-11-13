using log4net;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

namespace EV3.Dev.Csharp.Services.Remoting
{
    public class RemoteController : MarshalByRefObject, IRemoteController
    {
        private readonly ILog _log;
        private readonly BinaryServerFormatterSinkProvider _serverProvider;
        private bool _disposed;
        private TcpServerChannel _channel;
        private const int DefaultPort = 2000;

        public RemoteController()
        {
            _serverProvider = new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full };
        }

        public RemoteController(ILog log) :
            this()
        {
            _log = log;
        }

        private static IRemoteServices _remoteServices;

        public IRemoteServices RemoteServices => _remoteServices;

        public static IRemoteServices Connect() => Connect("LocalHost", 0);

        public static IRemoteServices Connect(string computerName) => Connect(computerName, 0);

        public static IRemoteServices Connect(string computerName, int port)
        {
            if (port == 0)
                port = DefaultPort;

            return ((RemoteController)Activator.GetObject(typeof(RemoteController), $@"tcp://{computerName}:{port}/RemoteController.rem")).RemoteServices;
        }

        public bool StartRemotingServer(IRemoteServices remoteServices, string ipAddress = null, int port = 0)
        {
            if (port == 0)
                port = DefaultPort;

            try
            {
                _log?.Info("Remote server starting...");
                _remoteServices = remoteServices;
                RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    var properties = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
                        {
                            {"name", nameof(RemoteController)},
                            {"bindTo", ipAddress},
                            {"port", port.ToString()}
                        };
                    _channel = new TcpServerChannel(properties, _serverProvider);
                }
                else
                {
                    _channel = new TcpServerChannel(nameof(RemoteController), DefaultPort, _serverProvider);
                }

                ChannelServices.RegisterChannel(_channel, false);
                NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;

                var remObj = new WellKnownServiceTypeEntry
                (
                    typeof(RemoteController),
                    "RemoteController.rem",
                    WellKnownObjectMode.Singleton
                );
                RemotingConfiguration.RegisterWellKnownServiceType(remObj);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            ResetRemotingChannels();
        }

        private void ResetRemotingChannels()
        {
            try
            {
                ChannelServices.UnregisterChannel(_channel);
                _channel = new TcpServerChannel(nameof(RemoteController), DefaultPort, _serverProvider);
                ChannelServices.RegisterChannel(_channel, false);
            }
            catch
            {
                // ignored
            }
        }

        public override object InitializeLifetimeService() => null;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _log?.Info("Disposing remote controller...");
            if (_channel != null)
                ChannelServices.UnregisterChannel(_channel);

            _disposed = disposing;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
