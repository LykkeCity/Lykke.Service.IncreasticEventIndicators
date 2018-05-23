using System;
using Autofac;
using Common.Log;

namespace Lykke.Service.IncreasticEventIndicators.Client
{
    public static class AutofacExtension
    {
        public static void RegisterIncreasticEventIndicatorsClient(this ContainerBuilder builder, string serviceUrl, ILog log)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterType<IncreasticEventIndicatorsClient>()
                .WithParameter("serviceUrl", serviceUrl)
                .As<IIncreasticEventIndicatorsClient>()
                .SingleInstance();
        }

        public static void RegisterIncreasticEventIndicatorsClient(this ContainerBuilder builder, IncreasticEventIndicatorsServiceClientSettings settings, ILog log)
        {
            builder.RegisterIncreasticEventIndicatorsClient(settings?.ServiceUrl, log);
        }
    }
}
