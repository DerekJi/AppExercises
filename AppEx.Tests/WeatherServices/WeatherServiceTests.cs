using AppEx.Services.CSV;
using AppEx.Services.Models;
using AppEx.Services.Weather;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AppEx.Tests.CsvServices
{
    public class WeatherServiceTests : TestFixtureBed
    {
        private IWeatherService localService { get; set; }

        public WeatherServiceTests(DependencyInjectionsFixture di, ITestOutputHelper testOutputHelper)
            : base(di, testOutputHelper)
        {
            localService = (IWeatherService)localServiceProvider.GetService(typeof(IWeatherService));
        }

        [Fact]
        public async Task GetJsonAsync_Test()
        {
            var response = await localService.GetJsonAsync(WeatherWmo.AdelaideAirport);
            Assert.True(response?.observations?.data?.Count > 0);
        }
    }
}
