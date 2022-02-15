using System;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using ShowNet.Models;

namespace ShowNet.Services
{
    public class DnsManager
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public DnsManager()
        {
        }

        public async Task<string> GetCurrentDnsStringAsync()
        {
            return await Task.Run(() =>
            {
                string returnResult = string.Empty;

                try
                {
                    var currentInterface = GetCurrentInterface();
                    if (currentInterface == null) throw new NullReferenceException();

                    var ipAddresses = currentInterface.GetIPProperties().DnsAddresses;
                    if (ipAddresses.Count <= 0) throw new InvalidOperationException();

                    returnResult = ipAddresses[0].ToString();
                }
                catch (NullReferenceException e)
                {
                    Logger.Error(e);
                }
                catch (InvalidOperationException e)
                {
                    Logger.Error(e);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }

                return returnResult;
            }).ConfigureAwait(false);
        }

        public string GetDnsProvider(string dnsString)
        {
            foreach (var dns in DnsInformation.AllDns)
            {
                if (dns.Value.Any(ip => ip == dnsString))
                    return dns.Key.ToString();
            }

            return string.Empty;
        }

        public async Task SetDnsAsync(DnsHosts host)
        {
            await Task.Run(() =>
            {
                try
                {
                    var currentInterface = GetCurrentInterface();
                    if (currentInterface == null) throw new NullReferenceException();

                    var managementCollection = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();

                    foreach (ManagementObject managementObject in managementCollection)
                    {
                        if (ManagementClassEnabledAndEquals(managementObject, currentInterface))
                        {
                            var dnsObject = managementObject.GetMethodParameters("SetDNSServerSearchOrder");
                            if (dnsObject is not null)
                            {
                                bool success = DnsInformation.AllDns.TryGetValue(host, out var dnsValues);
                                if (!success)
                                {
                                    Logger.Error("failed to get dnsHost"); // Default value if
                                    dnsValues = DnsInformation.AllDns[DnsHosts.Google]; // cast was not successful
                                }

                                dnsObject["DNSServerSearchOrder"] = dnsValues;
                                managementObject.InvokeMethod("SetDNSServerSearchOrder", dnsObject, null);
                            }
                        }
                    }
                }
                catch (NullReferenceException e)
                {
                    Logger.Error(e);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }).ConfigureAwait(false);
        }

        private bool ManagementClassEnabledAndEquals(ManagementObject mgObj, NetworkInterface inter)
        {
            return (bool) mgObj["IPEnabled"] &&
                   mgObj["Description"]
                       .ToString()
                       .Equals(inter.Description);
        }

        private NetworkInterface? GetCurrentInterface()
        {
            try
            {
                return NetworkInterface
                    .GetAllNetworkInterfaces()
                    .FirstOrDefault(i =>
                        i.OperationalStatus == OperationalStatus.Up &&
                        i.NetworkInterfaceType is NetworkInterfaceType.Wireless80211 or NetworkInterfaceType.Ethernet &&
                        i.GetIPProperties()
                            .GatewayAddresses
                            .Any(g =>
                                g.Address.AddressFamily.ToString() == "InterNetwork"));
            }
            catch (InvalidOperationException e)
            {
                Logger.Error(e);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return null;
        }
    }
}
