using AzureStorage;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;

namespace Lykke.Service.IncreasticEventIndicators.AzureRepositories
{
    public class LykkeIntrinsicEventIndicatorsRepository : IntrinsicEventIndicatorsRepository, ILykkeIntrinsicEventIndicatorsRepository
    {
        public LykkeIntrinsicEventIndicatorsRepository(INoSQLTableStorage<IntrinsicEventIndicatorsEntity> storage)
            : base(storage)
        {
        }
    }
}
