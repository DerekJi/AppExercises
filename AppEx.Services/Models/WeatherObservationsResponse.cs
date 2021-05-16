using Newtonsoft.Json;

namespace AppEx.Services.Models
{
    public class WeatherObservationsResponse
    {
        [JsonProperty("observations")]
        public WeatherObservations Observations { get; set; }
    }
}
