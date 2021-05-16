using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppEx.Services.Models
{
    public class WeatherObservationsResponse
    {
        [JsonProperty("observations")]
        public WeatherObservations Observations { get; set; }

        public List<WeatherRecordItem> GetRecords()
        {
            return Observations?.Records;
        }
    }
}
