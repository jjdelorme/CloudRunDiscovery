using System;
using Microsoft.AspNetCore.Http;

namespace Sample
{
    public interface IDiscovery 
    {
        string GetServiceUrl(HttpContext context, string service);
    }

    public class CloudRunDiscovery : IDiscovery
    {
        public string GetServiceUrl(HttpContext context, string service)
        {
            string fqdn = string.Empty;
            try
            {
                fqdn = context.Request.Headers["Host"];

                int lastDash = fqdn.LastIndexOf('-');
                int firstDot = fqdn.IndexOf('.');
                int beginDash = fqdn.LastIndexOf('-', lastDash - 1) + 1;
                string projectHash = fqdn.Substring(beginDash, (lastDash - beginDash) - 1);
                string region = fqdn.Substring(lastDash + 1, firstDot - lastDash - 1);
                string domain = fqdn.Substring(firstDot + 1, fqdn.Length - firstDot - 1);

                string serviceUrl = $"http://{service}-{projectHash}-{region}.{domain}";
                
                return serviceUrl;
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    $"Unable to identify Cloud Run pattern in Host header: {fqdn}", e);
            }
        }
    }
}