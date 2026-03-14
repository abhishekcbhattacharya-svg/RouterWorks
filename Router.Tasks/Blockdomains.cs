using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router.Tasks
{
    public class Blockdomains
    {
        private readonly IPAddressCollector iPAddressCollector;
        public Blockdomains(IPAddressCollector collector)
        {
            iPAddressCollector = collector;
        }

        public Dictionary<string, string> Domains(params string[] domains)
        {
            var dict = new Dictionary<string, string>();
            foreach (string domain in domains) 
            {
                var ips = iPAddressCollector.GetAddresses(domain);
                foreach (var ip in ips)
                {
                    dict.TryAdd(ip, domain);
                }
            }
            return dict;
        }
    }
}
