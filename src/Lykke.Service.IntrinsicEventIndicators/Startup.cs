using System;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.IntrinsicEventIndicators.AutoMapperProfiles;
using Lykke.Service.IntrinsicEventIndicators.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.IntrinsicEventIndicators
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "IntrinsicEventIndicators API",
            ApiVersion = "v1"
        };

        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider<AppSettings>(options =>
            {
                options.SwaggerOptions = _swaggerOptions;

                options.Logs = logs =>
                {
                    logs.AzureTableName = "IntrinsicEventIndicatorsLog";
                    logs.AzureTableConnectionStringResolver = settings => settings.IntrinsicEventIndicatorsService.Db.LogsConnString;
                };

                Mapper.Initialize(cfg =>
                {
                    cfg.AddProfiles(typeof(DefaultProfile));
                });
                Mapper.AssertConfigurationIsValid();
            });
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app)
        {
            app.UseLykkeConfiguration(options =>
            {
                options.SwaggerOptions = _swaggerOptions;
            });
        }
    }
}
