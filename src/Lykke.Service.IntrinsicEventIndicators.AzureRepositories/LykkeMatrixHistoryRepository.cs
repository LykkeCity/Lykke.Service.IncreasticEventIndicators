using AzureStorage;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;

namespace Lykke.Service.IntrinsicEventIndicators.AzureRepositories
{
    [UsedImplicitly]
    public class LykkeMatrixHistoryRepository : MatrixHistoryRepository, ILykkeMatrixHistoryRepository
    {
        public LykkeMatrixHistoryRepository(INoSQLTableStorage<MatrixHistoryEntity> storage)
            : base(storage)
        {
        }
    }
}
