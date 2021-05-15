using AppEx.Core.Attributes;
using AppEx.Services.Models;
using Microsoft.Extensions.Configuration;
using System;
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
        public async Task<WeatherObservationsResponse> FetchJsonAsync()
        {
            throw new NotImplementedException();
        }
    }
}
