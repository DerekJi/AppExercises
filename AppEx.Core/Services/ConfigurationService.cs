using AppEx.Core.Attributes;
using Microsoft.Extensions.Configuration;

namespace AppEx.Core.Services
{
    [Service(typeof(IConfigurationService))]
    public class ConfigurationService: IConfigurationService
    {
        public IConfiguration Configuration { get; }

        public ConfigurationService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
