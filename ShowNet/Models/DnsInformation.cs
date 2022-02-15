using System.Collections.Generic;

namespace ShowNet.Models
{
    public enum DnsHosts
    {
        Google,
        Cloudflare,
        Local
    }

    public class DnsInformation
    {
        public static Dictionary<DnsHosts, string[]?> AllDns = new()
        {
            {DnsHosts.Google, new[] {"8.8.8.8", "8.8.4.4"}},
            {DnsHosts.Cloudflare, new[] {"1.1.1.1", "1.0.0.1"}},
            {DnsHosts.Local, null}
        };
    }
}
