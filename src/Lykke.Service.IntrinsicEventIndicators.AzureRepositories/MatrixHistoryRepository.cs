using System;
using System.Threading.Tasks;
using AzureStorage;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;

namespace Lykke.Service.IntrinsicEventIndicators.AzureRepositories
{
    public class MatrixHistoryEntity : AzureTableEntity, IMatrixHistory
    {
        public string Data { get; set; }
        public DateTime Created { get; set; }
    }

    [UsedImplicitly]
    public abstract class MatrixHistoryRepository : IMatrixHistoryRepository
    {
        private readonly INoSQLTableStorage<MatrixHistoryEntity> _storage;

        protected MatrixHistoryRepository(INoSQLTableStorage<MatrixHistoryEntity> storage)
        {
            _storage = storage;
        }

        public async Task Save(IMatrixHistory matrixHistory)
        {
            var entity =
                new MatrixHistoryEntity
                {
                    PartitionKey = GeneratePartitionKey(matrixHistory),
                    RowKey = GenerateRowKey(matrixHistory),
                    Data = matrixHistory.Data,
                    Created = matrixHistory.Created
                };

            await _storage.InsertOrReplaceAsync(entity);
        }

        private static string GeneratePartitionKey(IMatrixHistory matrixHistory)
        {
            return $"{matrixHistory.Created:yyyy-MM-dd}";
        }

        private static string GenerateRowKey(IMatrixHistory matrixHistory)
        {
            return $"{matrixHistory.Created:HH:mm:ss.fffffff}";
        }
    }
}
