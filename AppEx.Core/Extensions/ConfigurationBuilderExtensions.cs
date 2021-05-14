using Microsoft.Extensions.Configuration;

namespace AppEx.Core.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddSettings(this IConfigurationBuilder builder, string rootPath, string environmentName, bool unitTest = false)
        {
            if (builder != null)
            {
                builder = builder.SetBasePath(rootPath)
                    // AppSettings
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                    ;

                return builder.AddEnvironmentVariables();
            }

            return builder;
        }
    }
}
