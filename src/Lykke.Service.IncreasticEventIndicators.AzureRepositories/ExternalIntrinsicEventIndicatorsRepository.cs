using AzureStorage;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;

namespace Lykke.Service.IntrinsicEventIndicators.AzureRepositories
{
    public class ExternalIntrinsicEventIndicatorsRepository : IntrinsicEventIndicatorsRepository, IExternalIntrinsicEventIndicatorsRepository
    {
        public ExternalIntrinsicEventIndicatorsRepository(INoSQLTableStorage<IntrinsicEventIndicatorsEntity> storage)
            : base(storage)
        {
        }
    }
}
