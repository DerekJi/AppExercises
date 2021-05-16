using AppEx.Core.Extensions;
using AppEx.Services.CSV;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace AppEx.Api
{
    public class Startup
    {
        private readonly string _allowedCorsOrigins = "*";

        public Startup(IWebHostEnvironment env)
        {
            var _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            Console.WriteLine($"EnvironmentName: {_environmentName}");

            var builder = new ConfigurationBuilder().AddSettings(env.ContentRootPath, _environmentName);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: _allowedCorsOrigins,
                    builder =>
                    {
                        var allowedHosts = Configuration.GetValue<string>("AllowedHosts");
                        if (!string.IsNullOrEmpty(allowedHosts))
                        {
                            builder.WithOrigins(allowedHosts.Split(';'))
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                        }

                    });
            });

            services.AddControllers()
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressConsumesConstraintForFormFileParameters = true;
                        options.SuppressInferBindingSourcesForParameters = true;
                        options.SuppressModelStateInvalidFilter = true;
                        options.SuppressMapClientErrors = true;
                        options.ClientErrorMapping[StatusCodes.Status404NotFound].Link =
                            "https://httpstatuses.com/404";
                    });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AppEx.Api", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.RegisterAllServices(Configuration)
                .AddScoped<ICsvService, CsvService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppEx.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(_allowedCorsOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
