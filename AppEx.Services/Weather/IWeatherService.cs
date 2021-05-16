using AppEx.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppEx.Services.Weather
{
    public interface IWeatherService
    {
        WeatherObservationsResponse _response { get; set; }

        Task<WeatherObservationsResponse> GetJsonAsync(WeatherWmo wmo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        double AverageTemperature(int hours = 72)
        {
            // Validations
            if (hours < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(hours), "The hours must be greater than 0");
            }
            var data = _response?.Observations?.Records;
            if (data?.Count < 1)
            {
                throw new Exception("Cannot found any weather observations");
            }

            // Calc sum
            var sum = 0.0;
            var maxSortOrder = hours * 2 - 1;
            data.OrderBy(x => x.SortOrder).ToList().ForEach(item =>
            {
                if (item.SortOrder <= maxSortOrder )
                {
                    sum += item.AirTemperature.HasValue ? item.AirTemperature.Value : 0.0;
                }                
            });

            //
            var average = sum / hours;

            return average;
        }
    }
}
