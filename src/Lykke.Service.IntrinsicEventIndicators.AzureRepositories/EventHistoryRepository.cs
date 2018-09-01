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
            var entity =
                new EventHistoryEntity
                {
                    PartitionKey = GeneratePartitionKey(eventHistory),
                    RowKey = GenerateRowKey(eventHistory.TickPriceTimestamp ?? DateTime.UtcNow),
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

        public async Task<IReadOnlyList<IEventHistory>> GetEventHistoryData(DateTime from, DateTime to, string exchange, string assetPair, decimal? delta)
        {
            return (await _storage.GetDataAsync()).ToArray();
        }

        private static string GeneratePartitionKey(IEventHistory eventHistory)
        {
            return $"{eventHistory.TickPriceTimestamp:yyyy-MM-dd}_{eventHistory.Exchange.ToUpperInvariant()}_{eventHistory.AssetPair.ToUpperInvariant()}_{eventHistory.Delta}";
        }

        private static string GenerateRowKey(DateTime date)
        {
            return $"{date:HH:mm:ss.fffffff}";
        }
    }
}
