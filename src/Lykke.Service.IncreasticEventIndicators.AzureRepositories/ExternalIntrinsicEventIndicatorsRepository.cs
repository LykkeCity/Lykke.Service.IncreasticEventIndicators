using AzureStorage;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;

namespace Lykke.Service.IncreasticEventIndicators.AzureRepositories
{
    public class ExternalIntrinsicEventIndicatorsRepository : IntrinsicEventIndicatorsRepository, IExternalIntrinsicEventIndicatorsRepository
    {
        public ExternalIntrinsicEventIndicatorsRepository(INoSQLTableStorage<IntrinsicEventIndicatorsEntity> storage)
            : base(storage)
        {
        }
    }
}
