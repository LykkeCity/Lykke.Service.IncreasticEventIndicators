using System;
using Autofac;
using Common.Log;

namespace Lykke.Service.IntrinsicEventIndicators.Client
{
    public static class AutofacExtension
    {
        public static void RegisterIntrinsicEventIndicatorsClient(this ContainerBuilder builder, string serviceUrl, ILog log)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterType<IntrinsicEventIndicatorsClient>()
                .WithParameter("serviceUrl", serviceUrl)
                .As<IIntrinsicEventIndicatorsClient>()
                .SingleInstance();
        }

        public static void RegisterIntrinsicEventIndicatorsClient(this ContainerBuilder builder, IntrinsicEventIndicatorsServiceClientSettings settings, ILog log)
        {
            builder.RegisterIntrinsicEventIndicatorsClient(settings?.ServiceUrl, log);
        }
    }
}
