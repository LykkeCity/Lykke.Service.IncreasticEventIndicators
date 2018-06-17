using AzureStorage;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;

namespace Lykke.Service.IntrinsicEventIndicators.AzureRepositories
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
