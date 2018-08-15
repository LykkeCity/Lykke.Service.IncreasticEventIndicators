using Autofac;
using AzureStorage.Tables;
using Common;
using Lykke.Common.Log;
using Lykke.Service.IntrinsicEventIndicators.AzureRepositories;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Services;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IntrinsicEventIndicators.Infrastructure;
using Lykke.Service.IntrinsicEventIndicators.Rabbit;
using Lykke.Service.IntrinsicEventIndicators.Services;
using Lykke.Service.IntrinsicEventIndicators.Services.Exchanges;
using Lykke.Service.IntrinsicEventIndicators.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.IntrinsicEventIndicators.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;

        public ServiceModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAutoMapper();

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
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            builder.Register(container => new LykkeIntrinsicEventIndicatorsRepository(
                    AzureTableStorage<IntrinsicEventIndicatorsEntity>
                        .Create(_settings.ConnectionString(x => x.IntrinsicEventIndicatorsService.Db.DataConnString), "LykkeIntrinsicEventIndicators", container.Resolve<ILogFactory>())))
                .As<ILykkeIntrinsicEventIndicatorsRepository>()
                .SingleInstance();

            builder.Register(container => new ExternalIntrinsicEventIndicatorsRepository(
                    AzureTableStorage<IntrinsicEventIndicatorsEntity>
                        .Create(_settings.ConnectionString(x => x.IntrinsicEventIndicatorsService.Db.DataConnString), "ExternalIntrinsicEventIndicators", container.Resolve<ILogFactory>())))
                .As<IExternalIntrinsicEventIndicatorsRepository>()
                .SingleInstance();

            builder.Register(container => new LykkeRunnerStateRepository(
                    AzureTableStorage<RunnerStateEntity>
                        .Create(_settings.ConnectionString(x => x.IntrinsicEventIndicatorsService.Db.DataConnString), "LykkeRunnersStates", container.Resolve<ILogFactory>())))
                .As<ILykkeRunnerStateRepository>()
                .SingleInstance();

            builder.Register(container => new ExternalRunnerStateRepository(
                    AzureTableStorage<RunnerStateEntity>
                        .Create(_settings.ConnectionString(x => x.IntrinsicEventIndicatorsService.Db.DataConnString), "ExternalRunnersStates", container.Resolve<ILogFactory>())))
                .As<IExternalRunnerStateRepository>()
                .SingleInstance();
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            builder.RegisterType<LykkeTickPriceSubscriber>()
                .As<IStartable>()
                .As<IStopable>()
                .WithParameter("settings", _settings.CurrentValue.IntrinsicEventIndicatorsService.LykkeTickPriceExchange)
                .AutoActivate()
                .SingleInstance();

            foreach (var tickPriceExchange in _settings.CurrentValue.IntrinsicEventIndicatorsService.ExternalTickPriceExchanges)
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
