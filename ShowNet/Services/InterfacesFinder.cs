using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace ShowNet.Services
{
    public interface IInterfacesFinder
    {
        List<Interface> GetInterfaces();
        void UpdateInterfaces();
    }

    public class InterfacesFinder : IInterfacesFinder
    {
        private List<Interface> _interfaces;

        public InterfacesFinder()
        {
            _interfaces = new List<Interface>();

            UpdateInterfaces();
        }

        public List<Interface> GetInterfaces()
        {
            return _interfaces;
        }

        public void UpdateInterfaces()
        {
            _interfaces = NetworkInterface
                .GetAllNetworkInterfaces()
                .Select(i => new Interface(i, GetIpVersions(i)))
                .ToList();
        }

        private string GetIpVersions(NetworkInterface inter)
        {
            string versionResult =
                inter.Supports(NetworkInterfaceComponent.IPv4)
                    ? "IPv4"
                    : "IPv4 not supported";

            versionResult +=
                inter.Supports(NetworkInterfaceComponent.IPv6)
                    ? "IPv6"
                    : "IPv6 not supported";

            return versionResult;
        }
    }

    public class Interface
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string InterfaceType { get; set; }
        public string OpStatus { get; set; }
        public string DnsEnabled { get; set; }
        public string IpVersion { get; set; }

        public Interface(
            string name,
            string description,
            string interfaceType,
            string opStatus,
            string dnsEnabled,
            string ipVersion)
        {
            Name = name;
            Description = description;
            InterfaceType = interfaceType;
            OpStatus = opStatus;
            DnsEnabled = dnsEnabled;
            IpVersion = ipVersion;
        }

        public Interface(NetworkInterface inter, string ipVersion)
        {
            Name = inter.Name;
            Description = inter.Description;
            InterfaceType = inter.NetworkInterfaceType.ToString();
            DnsEnabled = inter.GetIPProperties().IsDnsEnabled.ToString();
            IpVersion = ipVersion;
            OpStatus = inter.OperationalStatus.ToString();
        }
    }
}