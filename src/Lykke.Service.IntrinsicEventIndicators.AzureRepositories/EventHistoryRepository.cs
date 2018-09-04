using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.IntrinsicEventIndicators.AzureRepositories
{
    public class EventHistoryEntity : AzureTableEntity, IEventHistory
    {
        public int Event { get; set; }
        public decimal Extreme { get; set; }
        public decimal ExpectedDcLevel { get; set; }
        public decimal ExpectedOsLevel { get; set; }
        public decimal Reference { get; set; }
        public ExpectedDirectionalChange ExpectedDirectionalChange { get; set; }
        public decimal DirectionalChangePrice { get; set; }
        public decimal Delta { get; set; }
        public string AssetPair { get; set; }
        public string Exchange { get; set; }
        public decimal Ask { get; set; }
        public decimal Bid { get; set; }
        public DateTime? TickPriceTimestamp { get; set; }
        public DateTime? DcTimestamp { get; set; }

        public decimal IntrinsicEventIndicator { get; set; }
    }

    [UsedImplicitly]
    public abstract class EventHistoryRepository : IEventHistoryRepository
    {
        private readonly INoSQLTableStorage<EventHistoryEntity> _storage;

        protected EventHistoryRepository(INoSQLTableStorage<EventHistoryEntity> storage)
        {
            _storage = storage;
        }

        public async Task Save(IEventHistory eventHistory)
        {
            var now = DateTime.UtcNow;

            var entity =
                new EventHistoryEntity
                {
                    PartitionKey = GeneratePartitionKey(eventHistory.TickPriceTimestamp ?? now),
                    RowKey = GenerateRowKey(eventHistory.TickPriceTimestamp ?? now),
                    Event = eventHistory.Event,
                    Extreme = eventHistory.Extreme,
                    ExpectedDcLevel = eventHistory.ExpectedDcLevel,
                    ExpectedOsLevel = eventHistory.ExpectedOsLevel,
                    Reference = eventHistory.Reference,
                    ExpectedDirectionalChange = eventHistory.ExpectedDirectionalChange,
                    DirectionalChangePrice = eventHistory.DirectionalChangePrice,
                    AssetPair = eventHistory.AssetPair,
                    Delta = eventHistory.Delta,
                    Exchange = eventHistory.Exchange,
                    Ask = eventHistory.Ask,
                    Bid = eventHistory.Bid,
                    TickPriceTimestamp = eventHistory.TickPriceTimestamp,
                    DcTimestamp = eventHistory.DcTimestamp,
                    IntrinsicEventIndicator = eventHistory.IntrinsicEventIndicator
                };

            await _storage.InsertOrReplaceAsync(entity);
        }

        public async Task<IReadOnlyList<IEventHistory>> GetEventHistoryData(DateTime date, string exchange, string assetPair, decimal? delta)
        {
            var dateFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, GeneratePartitionKey(date));

            //var timeFilter = TableQuery.CombineFilters(
            //    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, GenerateRowKey(from)),
            //    TableOperators.And,
            //    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, GenerateRowKey(to))
            //);

            //var combinedFilter = TableQuery.CombineFilters(dateFilter, TableOperators.And, timeFilter);
            var combinedFilter = dateFilter;

            combinedFilter = !string.IsNullOrWhiteSpace(exchange)
                ? TableQuery.CombineFilters(combinedFilter, TableOperators.And, TableQuery.GenerateFilterCondition("Exchange", QueryComparisons.Equal, exchange))
                : combinedFilter;

            combinedFilter = !string.IsNullOrWhiteSpace(assetPair)
                ? TableQuery.CombineFilters(combinedFilter, TableOperators.And, TableQuery.GenerateFilterCondition("AssetPair", QueryComparisons.Equal, assetPair))
                : combinedFilter;

            combinedFilter = delta.HasValue
                ? TableQuery.CombineFilters(combinedFilter, TableOperators.And, TableQuery.GenerateFilterConditionForDouble("Delta", QueryComparisons.Equal, (double)delta.Value))
                : combinedFilter;

            var query = new TableQuery<EventHistoryEntity>().Where(combinedFilter);
            return (await _storage.WhereAsync(query)).ToArray();
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
