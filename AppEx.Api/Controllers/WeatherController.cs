using AppEx.Core.Extensions;
using AppEx.Services.CSV;
using AppEx.Services.CSV.Transform;
using AppEx.Services.Models;
using AppEx.Services.Weather;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AppEx.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherService _service;

        public WeatherController(
            IWeatherService service,
            ILogger<WeatherController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Provides weather observation data for any requested weather observation station in the Adelaide Area.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///    GET /api/v1/Weather/{stationId}
        /// </remarks>
        /// <param name="stationId">Id of the observation station</param>
        /// <param name="fields">The field(s) to be loaded, for example: "air_temp,press,weather" (NOTE: comman ',' should be used as splitter among multiple fields). All the observation data will be returned if 'fields' is empty or not populated</param>
        /// <returns></returns>
        [HttpGet("{stationId}")]
        public async Task<ActionResult> GetObservations(WeatherWmo stationId, string fields)
        {
            var result = await _service.GetJsonAsync(stationId);
            var records = result?.GetRecords();

            if (!string.IsNullOrEmpty(fields))
            {
                var splitter = ',';
                var expandos = _service.FilterBy(records, fields.Split(splitter));

                var jsonExpandos = JsonConvert.SerializeObject(expandos);
                return Ok(jsonExpandos);
            }
            else
            {
                var jsonRecords = JsonConvert.SerializeObject(records);
                return Ok(jsonRecords);
            }
        }
    }
}
