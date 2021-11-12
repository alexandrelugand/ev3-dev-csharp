using System;

namespace EV3.Dev.Csharp.Services.Remoting
{
    public interface IRemoteController : IDisposable
    {
        bool StartRemotingServer(IRemoteServices remoteServices, string ipAddress = null, int port = 0);
    }
}