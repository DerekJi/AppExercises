using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppEx.Services.Models
{
    public class WeatherObservations
    {
        [JsonProperty("data")]
        public List<WeatherRecordItem> Records { get; set; }
    }
}
