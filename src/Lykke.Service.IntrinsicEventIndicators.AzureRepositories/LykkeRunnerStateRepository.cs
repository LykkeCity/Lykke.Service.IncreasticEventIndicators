using AzureStorage;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;

namespace Lykke.Service.IntrinsicEventIndicators.AzureRepositories
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
