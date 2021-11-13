using System;
using System.Collections.Generic;

namespace EV3.Dev.Csharp.Services.Remoting
{
    public interface IRemoteServices : IDisposable
    {
        string Version { get; }
        List<string> AvailableServices { get; }
        IRemoteService GetService(string serviceName);
        void ReleaseService(IRemoteService service);
    }
}
