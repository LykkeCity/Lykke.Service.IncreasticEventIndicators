using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.AzureStorage.Tables;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;

namespace Lykke.Service.IncreasticEventIndicators.AzureRepositories
{
    public class IntrinsicEventIndicatorsEntity : AzureTableEntity, IIntrinsicEventIndicatorsColumn, IIntrinsicEventIndicatorsAssetPair
    {
        public string ColumnId => RowKey;
        public string RowId => RowKey;
        public decimal Delta { get; set; }
        public string AssetPair { get; set; }

        public static string GeneratePartitionKeyForColumn()
        {
            return "IntrinsicEventIndicatorsColumn";
        }

        public static string GeneratePartitionKeyForRow()
        {
            return "IntrinsicEventIndicatorsAssetPair";
        }

        public static IntrinsicEventIndicatorsEntity CreateForColumn(IIntrinsicEventIndicatorsColumn column)
        {
            return new IntrinsicEventIndicatorsEntity
            {
                PartitionKey = GeneratePartitionKeyForColumn(),
                RowKey = Guid.NewGuid().ToString(),
                Delta = column.Delta
            };
        }

        public static IntrinsicEventIndicatorsEntity CreateForAssetPair(IIntrinsicEventIndicatorsAssetPair row)
        {
            return new IntrinsicEventIndicatorsEntity
            {
                PartitionKey = GeneratePartitionKeyForRow(),
                RowKey = Guid.NewGuid().ToString(),
                AssetPair = row.AssetPair
            };
        }
    }

    public class IntrinsicEventIndicatorsRepository : IIntrinsicEventIndicatorsRepository
    {
        private readonly INoSQLTableStorage<IntrinsicEventIndicatorsEntity> _storage;

        public IntrinsicEventIndicatorsRepository(INoSQLTableStorage<IntrinsicEventIndicatorsEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IEnumerable<IIntrinsicEventIndicatorsColumn>> GetColumnsAsync()
        {
            return await _storage.GetDataAsync(IntrinsicEventIndicatorsEntity.GeneratePartitionKeyForColumn());
        }

        public async Task<IEnumerable<IIntrinsicEventIndicatorsAssetPair>> GetAssetPairsAsync()
        {
            return await _storage.GetDataAsync(IntrinsicEventIndicatorsEntity.GeneratePartitionKeyForRow());
        }

        public Task AddColumnAsync(IIntrinsicEventIndicatorsColumn column)
        {
            var entity = IntrinsicEventIndicatorsEntity.CreateForColumn(column);
            return _storage.InsertAsync(entity);
        }

        public async Task RemoveColumnAsync(string columnId)
        {
            await _storage.DeleteAsync(IntrinsicEventIndicatorsEntity.GeneratePartitionKeyForColumn(), columnId);
        }

        public Task AddAssetPairAsync(IIntrinsicEventIndicatorsAssetPair row)
        {
            var entity = IntrinsicEventIndicatorsEntity.CreateForAssetPair(row);
            return _storage.InsertAsync(entity);
        }

        public async Task RemoveAssetPairAsync(string rowId)
        {
            await _storage.DeleteAsync(IntrinsicEventIndicatorsEntity.GeneratePartitionKeyForRow(), rowId);
        }
    }
}
