using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace EV3.Dev.Csharp.Core.Helpers
{
    public static class NetworkInterfaceManager
    {
        static NetworkInterfaceManager()
        {
            Adapters = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var networkInterface in networkInterfaces)
            {
                var properties = networkInterface.GetIPProperties();
                var interfaceName = networkInterface.Name;
                var interfaceIp = properties.UnicastAddresses.FirstOrDefault(ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.Address.ToString();
                if (!Adapters.ContainsKey(interfaceName))
                    Adapters.Add(interfaceName, interfaceIp);
            }
        }

        public static IDictionary<string, string> Adapters { get; }

        public static string GetIpAddress(string interfaceName) => Adapters.ContainsKey(interfaceName) ? Adapters[interfaceName] : string.Empty;
    }
}
