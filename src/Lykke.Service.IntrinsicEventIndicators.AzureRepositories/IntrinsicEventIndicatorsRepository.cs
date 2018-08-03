using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.AzureStorage.Tables;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain.Model;

namespace Lykke.Service.IntrinsicEventIndicators.AzureRepositories
{
    public class IntrinsicEventIndicatorsEntity : AzureTableEntity, IIntrinsicEventIndicatorsColumn, IIntrinsicEventIndicatorsRow
    {
        public string ColumnId => RowKey;
        public string RowId => RowKey;
        public decimal Delta { get; set; }
        public string Exchange { get; set; }
        public string AssetPair { get; set; }
        public string PairName { get; set; }

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

        public static IntrinsicEventIndicatorsEntity CreateForAssetPair(IIntrinsicEventIndicatorsRow row)
        {
            return new IntrinsicEventIndicatorsEntity
            {
                PartitionKey = GeneratePartitionKeyForRow(),
                RowKey = Guid.NewGuid().ToString(),
                Exchange = row.Exchange,
                AssetPair = row.AssetPair,
                PairName = row.PairName
            };
        }
    }

    public abstract class IntrinsicEventIndicatorsRepository : IIntrinsicEventIndicatorsRepository
    {
        private readonly INoSQLTableStorage<IntrinsicEventIndicatorsEntity> _storage;

        protected IntrinsicEventIndicatorsRepository(INoSQLTableStorage<IntrinsicEventIndicatorsEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IEnumerable<IIntrinsicEventIndicatorsColumn>> GetColumnsAsync()
        {
            return await _storage.GetDataAsync(IntrinsicEventIndicatorsEntity.GeneratePartitionKeyForColumn());
        }

        public async Task<IEnumerable<IIntrinsicEventIndicatorsRow>> GetRowsAsync()
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

        public Task AddAssetPairAsync(IIntrinsicEventIndicatorsRow row)
        {
            var entity = IntrinsicEventIndicatorsEntity.CreateForAssetPair(row);
            return _storage.InsertAsync(entity);
        }

        public async Task EditAssetPairAsync(IIntrinsicEventIndicatorsRow row)
        {
            var entity = await _storage.GetDataAsync(IntrinsicEventIndicatorsEntity.GeneratePartitionKeyForRow(), row.RowId);
            entity.PairName = row.PairName;

            await _storage.ReplaceAsync(entity);
        }

        public async Task RemoveAssetPairAsync(string rowId)
        {
            await _storage.DeleteAsync(IntrinsicEventIndicatorsEntity.GeneratePartitionKeyForRow(), rowId);
        }
    }
}
