using AzureStorage;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;

namespace Lykke.Service.IntrinsicEventIndicators.AzureRepositories
{
    [UsedImplicitly]
    public class ExternalMatrixHistoryRepository : MatrixHistoryRepository, IExternalMatrixHistoryRepository
    {
        public ExternalMatrixHistoryRepository(INoSQLTableStorage<MatrixHistoryEntity> storage)
            : base(storage)
        {
        }
    }
}
