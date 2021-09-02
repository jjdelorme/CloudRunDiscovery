using System;
using System.Text.Json.Serialization;

namespace mvc.Models
{
    public class WeatherViewModel
    {
        [JsonPropertyName("summary")]
        public string Forecast { get; set; }

        [JsonPropertyName("temperatureF")]
        public int TemperatureF { get; set; }

        public string Temperature { get { return TemperatureF +"F"; } }
    }
}
