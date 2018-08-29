using System;
using System.Collections.Generic;
using System.Linq;
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
                    PartitionKey = GeneratePartitionKey(matrixHistory.Created),
                    RowKey = GenerateRowKey(matrixHistory.Created),
                    Data = matrixHistory.Data,
                    Created = matrixHistory.Created
                };

            await _storage.InsertOrReplaceAsync(entity);
        }

        public async Task<IList<DateTime>> GetMatrixHistoryStamps(DateTime date)
        {
            return (await _storage.GetDataAsync(GeneratePartitionKey(date)))
                .Select(x => x.Created).ToList();
        }

        public async Task<string> GetMatrixHistoryData(DateTime date)
        {
            return (await _storage.GetDataAsync(GeneratePartitionKey(date), GenerateRowKey(date)))
                .Data;
        }

        private static string GeneratePartitionKey(DateTime date)
        {
            return $"{date:yyyy-MM-dd}";
        }

        private static string GenerateRowKey(DateTime date)
        {
            return $"{date:HH:mm:ss.fffffff}";
        }
    }
}
