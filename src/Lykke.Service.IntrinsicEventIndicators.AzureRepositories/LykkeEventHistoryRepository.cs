using AzureStorage;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;

namespace Lykke.Service.IntrinsicEventIndicators.AzureRepositories
{
    [UsedImplicitly]
    public class LykkeEventHistoryRepository : EventHistoryRepository, ILykkeEventHistoryRepository
    {
        public LykkeEventHistoryRepository(INoSQLTableStorage<EventHistoryEntity> storage)
            : base(storage)
        {
        }
    }
}
