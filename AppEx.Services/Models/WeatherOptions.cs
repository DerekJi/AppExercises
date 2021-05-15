using System.Collections.Generic;

namespace AppEx.Services.Models
{
    public class WeatherOptions
    {
        /// <summary>
        /// Specify the key name in appsettings.json
        /// </summary>
        public string AppSettingKey { get; } = "Weather";

        /// <summary>
        /// URL of the CSV file
        /// </summary>
        public string JsonUrl { get; set; }
    }
}
