using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDiscovery, CloudRunDiscovery>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    string service = "myservice";
                    string url = "";
                    if (!env.IsDevelopment())
                        url = GetMyServiceUrl(context, service);
                    else
                        url = service;
                        
                    await context.Response.WriteAsync($"<html>Hello, the url is {url}</html>");
                });
            });
        }

        private string GetMyServiceUrl(HttpContext context, string service)
        {
            var discovery = context.RequestServices.GetRequiredService<IDiscovery>();
            return discovery.GetServiceUrl(context, service);
        }
    }
}
