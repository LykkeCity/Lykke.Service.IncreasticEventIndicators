using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;

namespace Lykke.Service.IncreasticEventIndicators.AzureRepositories
{
    public class RunnerStateEntity : AzureTableEntity, IRunnerState
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
        public string Version { get; set; }
    }

    [UsedImplicitly]
    public class RunnerStateRepository : IRunnerStateRepository
    {
        private readonly INoSQLTableStorage<RunnerStateEntity> _storage;

        public RunnerStateRepository(INoSQLTableStorage<RunnerStateEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IReadOnlyList<IRunnerState>> GetState()
        {
            return (await _storage.GetDataAsync()).ToArray();
        }

        public async Task SaveState(IReadOnlyList<IRunnerState> state)
        {
            var version = Guid.NewGuid().ToString();

            var entities = state.Select(x =>
                new RunnerStateEntity
                {
                    PartitionKey = GeneratePartitionKey(x),
                    RowKey = GenerateRowKey(x),
                    Event = x.Event,
                    Extreme = x.Extreme,
                    ExpectedDcLevel = x.ExpectedDcLevel,
                    ExpectedOsLevel = x.ExpectedOsLevel,
                    Reference = x.Reference,
                    ExpectedDirectionalChange = x.ExpectedDirectionalChange,
                    DirectionalChangePrice = x.DirectionalChangePrice,
                    Version = version
                }
            ).ToArray();

            await InsertOrReplaceAsync(entities, version);
        }

        private async Task InsertOrReplaceAsync(IEnumerable<RunnerStateEntity> entities, string version)
        {
            var tasks = entities.Select(x => _storage.InsertOrReplaceAsync(x)).ToArray();
            await Task.WhenAll(tasks);

            var entitiesToDelete = await _storage.GetDataAsync(x => x.Version != version);

            tasks = entitiesToDelete.Select(x => _storage.DeleteAsync(x)).ToArray();
            await Task.WhenAll(tasks);
        }

        private static string GeneratePartitionKey(IRunnerState runnerState)
        {
            return runnerState.AssetPair;
        }

        private static string GenerateRowKey(IRunnerState runnerState)
        {
            return runnerState.Delta.ToString(CultureInfo.InvariantCulture);
        }
    }
}
