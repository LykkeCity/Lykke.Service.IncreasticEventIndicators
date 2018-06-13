using AzureStorage;
using JetBrains.Annotations;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;

namespace Lykke.Service.IncreasticEventIndicators.AzureRepositories
{
    [UsedImplicitly]
    public class LykkeRunnerStateRepository : RunnerStateRepository, ILykkeRunnerStateRepository
    {
        public LykkeRunnerStateRepository(INoSQLTableStorage<RunnerStateEntity> storage)
            : base(storage)
        {
        }
    }
}
