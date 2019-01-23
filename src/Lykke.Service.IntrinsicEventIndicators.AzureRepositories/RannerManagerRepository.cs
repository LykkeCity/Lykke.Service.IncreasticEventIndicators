using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.LyciAssets;

namespace Lykke.Service.IntrinsicEventIndicators.AzureRepositories
{
    public class RannerManagerRepository : IRannerManagerRepository
    {
        private readonly INoSQLTableStorage<RunnerStateEntity> _storage;

        public RannerManagerRepository(INoSQLTableStorage<RunnerStateEntity> storage)
        {
            _storage = storage;
        }

        public async Task<List<RunnerState>> GetAllAsync()
        {
            var data = await _storage.GetDataAsync();

            var result = data.Select(runnerStateEntity => new RunnerState(runnerStateEntity.Event, runnerStateEntity.Extreme,
                runnerStateEntity.ExpectedDcLevel, runnerStateEntity.ExpectedOsLevel, runnerStateEntity.Reference,
                runnerStateEntity.ExpectedDirectionalChange, runnerStateEntity.DirectionalChangePrice,
                runnerStateEntity.Delta, runnerStateEntity.AssetPair, runnerStateEntity.Exchange,
                runnerStateEntity.Ask, runnerStateEntity.Bid, runnerStateEntity.TickPriceTimestamp,
                runnerStateEntity.DcTimestamp)).ToList();


            return result;
        }

        public async Task SaveAllAsync(IEnumerable<IRunnerState> runners)
        {
            var entities = runners.Select(x =>
                new RunnerStateEntity
                {
                    PartitionKey = "LyciRanner",
                    RowKey = x.AssetPair,
                    Event = x.Event,
                    Extreme = x.Extreme,
                    ExpectedDcLevel = x.ExpectedDcLevel,
                    ExpectedOsLevel = x.ExpectedOsLevel,
                    Reference = x.Reference,
                    ExpectedDirectionalChange = x.ExpectedDirectionalChange,
                    DirectionalChangePrice = x.DirectionalChangePrice,
                    AssetPair = x.AssetPair,
                    Delta = x.Delta,
                    Exchange = x.Exchange,
                    Ask = x.Ask,
                    Bid = x.Bid,
                    TickPriceTimestamp = x.TickPriceTimestamp,
                    DcTimestamp = x.DcTimestamp
                }
            ).ToArray();

            for (int i = 0; i <= entities.Length / 50; i++)
            {
                var data = entities.Skip(i * 50).Take(50);
                var tasks = data.Select(e => _storage.InsertOrReplaceAsync(e)).ToArray();
                await Task.WhenAll(tasks);
            }
        }
    }
}
