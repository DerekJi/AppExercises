using AppEx.Core.Extensions;
using AppEx.Services.CSV.Transform;
using AppEx.Services.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AppEx.Services.Weather
{
    public interface IWeatherService
    {
        WeatherObservationsResponse _response { get; set; }

        Task<WeatherObservationsResponse> GetJsonAsync(WeatherWmo wmo);

        /// <summary>
        /// Filter records by given fields
        /// </summary>
        /// <param name="records"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        IEnumerable<ExpandoObject> FilterBy(List<WeatherRecordItem> records, params string[] fields)
        {
            if (records?.Count < 1 || fields?.Length < 1)
            {
                return null;
            }

            var fieldsList = fields.ToList();            

            var expandos = records.Select(item =>
            {
                var expando = item.ToExpando();
                var keys = expando.GetKeys().ToList();
                var removeColumns = new List<string>();
                foreach (var key in keys)
                {
                    if (!fieldsList.Contains(key))
                    {
                        removeColumns.Add(key);
                    }
                }

                if (removeColumns.Count > 0)
                {
                    var options = new TransformOptions()
                    {
                        RemoveColumns = removeColumns
                    };
                    new RemoveColumnStrategy().Transform(options, expando);
                }
                return expando;
            });

            return expandos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        double AverageTemperature(List<WeatherRecordItem> records, int hours = 72)
        {
            // Validations
            if (hours < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(hours), "The hours must be greater than 0");
            }
            if (records?.Count < 1)
            {
                throw new ArgumentOutOfRangeException("Cannot found any weather observations");
            }

            // Calc sum
            var recordsPerHour = 2;
            var sum = 0.0;
            var maxSortOrder = hours * recordsPerHour - 1;
            records.OrderBy(x => x.SortOrder).ToList().ForEach(item =>
            {
                if (item.SortOrder <= maxSortOrder )
                {
                    sum += item.AirTemperature.HasValue ? item.AirTemperature.Value : 0.0;
                }                
            });

            //
            var average = sum / (maxSortOrder + 1);

            return average;
        }
    }
}
