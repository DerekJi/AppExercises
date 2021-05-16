using AppEx.Core.Extensions;
using AppEx.Services.Models;
using AppEx.Services.Weather;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppEx.Weather.ConsoleCli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            Console.WriteLine("Welcome to Application Exercise - Weather");
            Console.WriteLine("Are you sure to load the remote weather observation file? (y/N)");
            var yesOrNo = Console.ReadKey();
            if (new List<char>() { 'Y', 'y' }.Contains(yesOrNo.KeyChar))
            {
                await RunCsvConsole(host.Services);
            }
        }

        static async Task RunCsvConsole(IServiceProvider services)
        {
            Console.WriteLine("\r\nDownloading JSON file ... please wait.");
            var csv = services.GetRequiredService<IWeatherService>();
            var response = await csv.GetJsonAsync(WeatherWmo.AdelaideAirport);

            Console.WriteLine("\r\nJSON downloaded, anylizing ... please wait.");
            var hours = 72;
            var avgTemp = csv.AverageTemperature(response?.GetRecords(), hours);
            Console.WriteLine($"The average temparature of last {hours} hours is: {avgTemp.ToString("F")}°C");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    var _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
                    var _settingsBasePath = Environment.CurrentDirectory + "/../../../../AppEx.Api";
                    var builder = new ConfigurationBuilder().AddSettings(_settingsBasePath, _environmentName, true);
                    var configuration = builder.Build();

                    services
                        .RegisterAllServices(configuration)
                        .AddScoped<IWeatherService, WeatherService>();
                });
    }
}
