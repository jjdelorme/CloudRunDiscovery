using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace mvc
{
    public interface IDiscovery 
    {
        string GetServiceUrl(HttpContext context, string service);
    }

    public class CloudRunDiscovery : IDiscovery
    {
        private readonly IWebHostEnvironment _env;

        public CloudRunDiscovery(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GetServiceUrl(HttpContext context, string service)
        {           
            if (_env.IsDevelopment())
                return $"http://localhost:5000";

            string fqdn = "";
            try
            {
                fqdn = context.Request.Headers["Host"];

                int lastDash = fqdn.LastIndexOf('-');
                int firstDot = fqdn.IndexOf('.');
                int beginDash = fqdn.LastIndexOf('-', lastDash - 1) + 1;
                string projectHash = fqdn.Substring(beginDash, lastDash - beginDash);
                string region = fqdn.Substring(lastDash + 1, firstDot - lastDash - 1);
                string domain = fqdn.Substring(firstDot + 1, fqdn.Length - firstDot - 1);

                string serviceUrl = $"https://{service}-{projectHash}-{region}.{domain}";
                
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