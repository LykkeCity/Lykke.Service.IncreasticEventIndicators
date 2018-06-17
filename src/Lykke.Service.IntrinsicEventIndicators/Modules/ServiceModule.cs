using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common;
using Common.Log;
using Lykke.Service.IntrinsicEventIndicators.AzureRepositories;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Services;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IntrinsicEventIndicators.Rabbit;
using Lykke.Service.IntrinsicEventIndicators.Services;
using Lykke.Service.IntrinsicEventIndicators.Services.Exchanges;
using Lykke.Service.IntrinsicEventIndicators.Settings.ServiceSettings;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.IntrinsicEventIndicators.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<IntrinsicEventIndicatorsSettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<IntrinsicEventIndicatorsSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.RegisterInstance(_settings.CurrentValue)
                .SingleInstance();

            builder.RegisterType<LykkeTickPriceManager>()
                .As<ILykkeTickPriceManager>()
                .As<ILykkeTickPriceHandler>()
                .SingleInstance();

            builder.RegisterType<ExternalTickPriceManager>()
                .As<IExternalTickPriceManager>()
                .As<IExternalTickPriceHandler>()
                .SingleInstance();

            builder.RegisterType<LykkeIntrinsicEventIndicatorsService>()
                .As<ILykkeIntrinsicEventIndicatorsService>()
                .SingleInstance();

            builder.RegisterType<ExternalIntrinsicEventIndicatorsService>()
                .As<IExternalIntrinsicEventIndicatorsService>()
                .SingleInstance();

            RegisterRepositories(builder);
            RegisterRabbitMqSubscribers(builder);

            builder.Populate(_services);
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<LykkeIntrinsicEventIndicatorsRepository>()
                .As<ILykkeIntrinsicEventIndicatorsRepository>()
                .WithParameter(TypedParameter.From(AzureTableStorage<IntrinsicEventIndicatorsEntity>
                    .Create(_settings.ConnectionString(x => x.Db.DataConnString), "LykkeIntrinsicEventIndicators", _log)))
                .SingleInstance();

            builder.RegisterType<ExternalIntrinsicEventIndicatorsRepository>()
                .As<IExternalIntrinsicEventIndicatorsRepository>()
                .WithParameter(TypedParameter.From(AzureTableStorage<IntrinsicEventIndicatorsEntity>
                    .Create(_settings.ConnectionString(x => x.Db.DataConnString), "ExternalIntrinsicEventIndicators", _log)))
                .SingleInstance();

            builder.RegisterType<LykkeRunnerStateRepository>()
                .As<ILykkeRunnerStateRepository>()
                .WithParameter(TypedParameter.From(AzureTableStorage<RunnerStateEntity>
                    .Create(_settings.ConnectionString(x => x.Db.DataConnString), "LykkeRunnersStates", _log)))
                .SingleInstance();

            builder.RegisterType<ExternalRunnerStateRepository>()
                .As<IExternalRunnerStateRepository>()
                .WithParameter(TypedParameter.From(AzureTableStorage<RunnerStateEntity>
                    .Create(_settings.ConnectionString(x => x.Db.DataConnString), "ExternalRunnersStates", _log)))
                .SingleInstance();
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            builder.RegisterType<LykkeTickPriceSubscriber>()
                .As<IStartable>()
                .As<IStopable>()
                .WithParameter("settings", _settings.CurrentValue.LykkeTickPriceExchange)
                .AutoActivate()
                .SingleInstance();

            foreach (var tickPriceExchange in _settings.CurrentValue.ExternalTickPriceExchanges)
            {
                builder.RegisterType<ExternalTickPriceSubscriber>()
                    .As<IStartable>()
                    .As<IStopable>()
                    .WithParameter("settings", tickPriceExchange)
                    .AutoActivate()
                    .SingleInstance();
            }
        }
    }
}
