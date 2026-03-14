namespace Router.Tasks
{
    public class IPAddressCollector
    {
        public IEnumerable<string> GetAddresses(string domain)
        {
            try
            {
                return _Addresses(domain);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving IP addresses: {ex.Message}");
            }
            return Enumerable.Empty<string>();
        }

        private IEnumerable<string> _Addresses(string domain)
        {
            var ipHostEntry = System.Net.Dns.GetHostAddresses(domain,System.Net.Sockets.AddressFamily.Unspecified);
            foreach (System.Net.IPAddress ipAddress in ipHostEntry)
            {
                yield return ipAddress.ToString();
            }
        }
    }
}
