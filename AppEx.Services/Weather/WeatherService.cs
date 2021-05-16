using AppEx.Core.Attributes;
using AppEx.Services.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AppEx.Services.Weather
{
    [Service(typeof(IWeatherService))]
    public class WeatherService : IWeatherService
    {
        protected IConfiguration Configuration { get; set; }
        protected WeatherOptions Options { get; }
        public WeatherObservationsResponse _response { get; set; }

        public WeatherService(IConfiguration configuration)
        {
            Options = new WeatherOptions();
            Configuration = configuration;
            Configuration.GetSection(Options.AppSettingKey).Bind(Options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<WeatherObservationsResponse> GetJsonAsync(WeatherWmo wmo)
        {
            if (string.IsNullOrEmpty(Options?.JsonUrl))
            {
                throw new Exception("Cannot find Json Url from settings.");
            }

            var jsonUrl = Options.JsonUrl.Replace("{WMO}", ((int)wmo).ToString());
            using (var wc = new WebClient())
            {
                wc.Headers.Add("User-Agent", "Other");
                var url = new Uri(jsonUrl);
                var content = wc.DownloadString(url);
                var result = JsonConvert.DeserializeObject<WeatherObservationsResponse>(content);
                return await Task.FromResult(result);
            }
        }
    }
}
