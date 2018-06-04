using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;
using Lykke.Service.IncreasticEventIndicators.Core.Services;

namespace Lykke.Service.IncreasticEventIndicators.Services
{
    public class IntrinsicEventIndicatorsService : IIntrinsicEventIndicatorsService
    {
        private readonly IIntrinsicEventIndicatorsRepository _repo;
        private readonly ILog _log;
        private readonly ITickPriceManager _tickPriceManager;

        private bool _initialized;

        public IntrinsicEventIndicatorsService(IIntrinsicEventIndicatorsRepository repo,
            ITickPriceManager tickPriceManager, ILog log)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _log = log.CreateComponentScope(nameof(IntrinsicEventIndicatorsService));
            _tickPriceManager = tickPriceManager;

            EnsureInitialized();
        }

        public async Task AddColumn(IIntrinsicEventIndicatorsColumn column)
        {
            EnsureInitialized();

            await _repo.AddColumnAsync(column);
            await UpdateRunners();
        }

        public async Task RemoveColumn(string columnId)
        {
            EnsureInitialized();

            await _repo.RemoveColumnAsync(columnId);
            await UpdateRunners();
        }

        public async Task AddAssetPair(IIntrinsicEventIndicatorsAssetPair row)
        {
            EnsureInitialized();

            await _repo.AddAssetPairAsync(row);
            await UpdateRunners();
        }

        public async Task RemoveAssetPair(string rowId)
        {
            EnsureInitialized();

            await _repo.RemoveAssetPairAsync(rowId);
            await UpdateRunners();
        }

        public async Task<IntrinsicEventIndicators> GetData()
        {
            EnsureInitialized();

            var rows = (await _repo.GetAssetPairsAsync()).OrderBy(x => x.AssetPair).ToList();
            var columns = (await _repo.GetColumnsAsync()).OrderBy(x => x.Delta).ToList();            

            var data = await _tickPriceManager.GetIntrinsicEventIndicators(
                rows.Select(x => x.AssetPair).ToList(),
                columns.Select(x => x.Delta).ToList());

            return await Task.FromResult(new IntrinsicEventIndicators
            {
                Columns = columns.ToArray(),
                AssetPairs = rows.ToArray(),
                Data = data
            });
        }

        public Task<IDictionary<string, IList<IRunnerState>>> GetRunnersStates()
        {
            EnsureInitialized();

            return _tickPriceManager.GetRunnersStates();
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;

            var task = Task.Run(UpdateRunners);
            Task.WaitAll(task);

            _initialized = true;
        }

        private async Task UpdateRunners()
        {
            var columns = (await _repo.GetColumnsAsync()).Select(x => x.Delta).OrderBy(x => x).ToList();
            var rows = (await _repo.GetAssetPairsAsync()).Select(x => x.AssetPair).OrderBy(x => x).ToList();

            await _tickPriceManager.UpdateRunners(rows, columns);
        }
    }
}
