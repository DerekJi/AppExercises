using AppEx.Services.CSV;
using AppEx.Services.Models;
using AppEx.Services.Weather;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AppEx.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet("{wmo}")]
        public async Task<ActionResult> GetObservations(WeatherWmo wmo)
        {
            var result = await _service.FetchJsonAsync(wmo);

            return Ok(result);
        }
    }
}
