using System.Collections.Generic;

namespace ShowNet.Models
{
    public enum DnsHosts
    {
        Google,
        Cloudflare
    }

    public class DnsInformation
    {
        public static Dictionary<DnsHosts, List<string>> AllDns = new()
        {
            {DnsHosts.Google, new List<string> {"8.8.8.8", "8.8.4.4"}},
            {DnsHosts.Cloudflare, new List<string> {"1.1.1.1", "1.0.0.1"}}
        };
    }
}
