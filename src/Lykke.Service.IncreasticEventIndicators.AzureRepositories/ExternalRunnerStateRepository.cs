using AzureStorage;
using JetBrains.Annotations;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;

namespace Lykke.Service.IncreasticEventIndicators.AzureRepositories
{
    [UsedImplicitly]
    public class ExternalRunnerStateRepository : RunnerStateRepository, IExternalRunnerStateRepository
    {
        public ExternalRunnerStateRepository(INoSQLTableStorage<RunnerStateEntity> storage)
            : base(storage)
        {
        }
    }
}
