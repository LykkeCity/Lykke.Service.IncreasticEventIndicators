using AzureStorage;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;

namespace Lykke.Service.IntrinsicEventIndicators.AzureRepositories
{
    [UsedImplicitly]
    public class ExternalEventHistoryRepository : EventHistoryRepository, IExternalEventHistoryRepository
    {
        public ExternalEventHistoryRepository(INoSQLTableStorage<EventHistoryEntity> storage)
            : base(storage)
        {
        }
    }
}
