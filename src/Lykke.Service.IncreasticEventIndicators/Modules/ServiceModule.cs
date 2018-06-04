using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common;
using Common.Log;
using Lykke.Service.IncreasticEventIndicators.AzureRepositories;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;
using Lykke.Service.IncreasticEventIndicators.Core.Services;
using Lykke.Service.IncreasticEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IncreasticEventIndicators.Rabbit;
using Lykke.Service.IncreasticEventIndicators.Settings.ServiceSettings;
using Lykke.Service.IncreasticEventIndicators.Services;
using Lykke.Service.IncreasticEventIndicators.Services.Exchanges;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.IncreasticEventIndicators.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<IncreasticEventIndicatorsSettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<IncreasticEventIndicatorsSettings> settings, ILog log)
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

            builder.RegisterType<LykkeTickPriceSubscriber>()
                .As<IStartable>()
                .As<IStopable>()
                .AutoActivate()
                .SingleInstance();

            builder.RegisterType<TickPriceManager>()
                .As<ITickPriceManager>()
                .As<ILykkeTickPriceHandler>()
                .SingleInstance();

            builder.RegisterType<IntrinsicEventIndicatorsService>()
                .As<IIntrinsicEventIndicatorsService>()
                .SingleInstance();

            RegisterRepositories(builder);

            builder.Populate(_services);
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<IntrinsicEventIndicatorsRepository>()
                .As<IIntrinsicEventIndicatorsRepository>()
                .WithParameter(TypedParameter.From(AzureTableStorage<IntrinsicEventIndicatorsEntity>
                    .Create(_settings.ConnectionString(x => x.Db.DataConnString), "IntrinsicEventIndicators", _log)))
                .SingleInstance();

            builder.RegisterType<RunnerStateRepository>()
                .As<IRunnerStateRepository>()
                .WithParameter(TypedParameter.From(AzureTableStorage<RunnerStateEntity>
                    .Create(_settings.ConnectionString(x => x.Db.DataConnString), "RunnersStates", _log)))
                .SingleInstance();
        }
    }
}
