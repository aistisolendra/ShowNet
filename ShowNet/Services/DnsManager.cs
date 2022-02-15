using System;
using System.Linq;
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

        private NetworkInterface GetCurrentInterface()
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
