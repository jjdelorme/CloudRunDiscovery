using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net.Http;
using mvc.Models;

namespace mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDiscovery _discovery;
        private readonly IHttpClientFactory _httpFactory;

        public HomeController(ILogger<HomeController> logger, IDiscovery discovery, IHttpClientFactory httpFactory)
        {
            _logger = logger;
            _discovery = discovery;
            _httpFactory = httpFactory;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetWeatherAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<WeatherViewModel> GetWeatherAsync()
        {
            WeatherViewModel model = null;

            try
            {
                string url = _discovery.GetServiceUrl(HttpContext, "weatherapi")
                    + "/WeatherForecast";
            
                var client = _httpFactory.CreateClient();
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    IEnumerable<WeatherViewModel> forecast = await JsonSerializer.DeserializeAsync
                        <IEnumerable<WeatherViewModel>>(responseStream);

                    model = forecast.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not retrieve weather from api.");
            }

            if (model == null)
            {
                model = new WeatherViewModel() { Forecast = "Sunny", TemperatureF = 72 };
            }

            return model;
        }
    }
}
