using AppEx.Core.Extensions;
using AppEx.Services.CSV;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppEx.Csv.ConsoleCli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            Console.WriteLine("\r\nWelcome to Application Exercise - CSV");
            Console.WriteLine("\r\nAre you sure to load the remote CSV file? (y/N)");
            var yesOrNo = Console.ReadKey();
            if (new List<char>() { 'Y', 'y' }.Contains(yesOrNo.KeyChar))
            {
                await RunCsvConsole(host.Services);
            }
        }

        static async Task RunCsvConsole(IServiceProvider services)
        {
            Console.WriteLine("\r\nDownloading CSV file ... please wait.");
            var csv = services.GetRequiredService<ICsvService>();

            var content = await csv.FetchRecordsAsync(true);
            if (content?.Count > 0)
            {
                Console.WriteLine("\r\nCSV file has been loaded successfully.");
                Console.WriteLine("\r\nPlease type the name of the transformed CSV: ");
                var filename = Console.ReadLine();
                if (string.IsNullOrEmpty(filename))
                {
                    Console.WriteLine("\r\nThe filename should not be empty");
                }
                else
                {
                    Console.WriteLine("\r\nWriting file ... please wait.");
                    var path = csv.SaveAs(content, filename, true);
                    Console.WriteLine($"\r\nFile saved: {path}");
                }
            }
            else
            {
                Console.WriteLine("\r\nCannot find any data from the source URL");
            }
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
                        .AddScoped<ICsvService, CsvService>();
                });
    }
}
