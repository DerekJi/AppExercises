using AppEx.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AppEx.Tests
{
    public class DependencyInjectionsFixture
    {
        public IServiceProvider ServiceProvider { get; private set; }
        private IConfiguration Configuration { get; set; }
        private readonly string _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        private readonly string _settingsBasePath = Environment.CurrentDirectory + "/../../../../AppEx.Csv.Api";

        /// <summary>
        /// 
        /// </summary>
        public DependencyInjectionsFixture()
        {
            var builder = new ConfigurationBuilder().AddSettings(_settingsBasePath, _environmentName, true);
            Configuration = builder.Build();

            var services = new ServiceCollection();
            services
                .RegisterAllServices(Configuration)
                ;

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
