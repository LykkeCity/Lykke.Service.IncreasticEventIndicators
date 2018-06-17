using AzureStorage;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;

namespace Lykke.Service.IntrinsicEventIndicators.AzureRepositories
{
    public class LykkeIntrinsicEventIndicatorsRepository : IntrinsicEventIndicatorsRepository, ILykkeIntrinsicEventIndicatorsRepository
    {
        public LykkeIntrinsicEventIndicatorsRepository(INoSQLTableStorage<IntrinsicEventIndicatorsEntity> storage)
            : base(storage)
        {
        }
    }
}
