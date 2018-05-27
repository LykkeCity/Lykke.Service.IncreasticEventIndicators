using System;
using System.Collections.Generic;
using System.Globalization;
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

        public IntrinsicEventIndicatorsService(IIntrinsicEventIndicatorsRepository repo,
            ITickPriceManager tickPriceManager, ILog log)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _log = log.CreateComponentScope(nameof(IntrinsicEventIndicatorsService));
            _tickPriceManager = tickPriceManager;
        }

        public async Task AddColumn(IIntrinsicEventIndicatorsColumn column)
        {
            await _repo.AddColumnAsync(column);
            await UpdateRunners();
        }

        public async Task RemoveColumn(string columnId)
        {
            await _repo.RemoveColumnAsync(columnId);
            await UpdateRunners();
        }

        public async Task AddAssetPair(IIntrinsicEventIndicatorsAssetPair row)
        {
            await _repo.AddAssetPairAsync(row);
            await UpdateRunners();
        }

        public async Task RemoveAssetPair(string rowId)
        {
            await _repo.RemoveAssetPairAsync(rowId);
            await UpdateRunners();
        }

        public async Task<IntrinsicEventIndicators> GetData()
        {
            var columns = (await _repo.GetColumnsAsync()).OrderBy(x => x.Delta).ToList();
            var rows = (await _repo.GetAssetPairsAsync()).OrderBy(x => x.AssetPair).ToList();

            var data = rows.Select(x => columns.Select(c => 0M.ToString(CultureInfo.InvariantCulture)).ToArray()).ToArray();

            var runners = new Dictionary<string, Runner>();

            foreach (var row in rows)
            {
                var assetPair = row.AssetPair;

                foreach (var column in columns)
                {
                    var delta = (double)column.Delta;

                    var runner = new Runner(delta, delta, delta, delta, 0);

                    runners.Add(assetPair, runner);
                }
            }

            return await Task.FromResult(new IntrinsicEventIndicators
            {
                Columns = columns.ToArray(),
                AssetPairs = rows.ToArray(),
                Data = data
            });
        }

        private async Task UpdateRunners()
        {
            var columns = (await _repo.GetColumnsAsync()).Select(x => x.Delta).OrderBy(x => x).ToList();
            var rows = (await _repo.GetAssetPairsAsync()).Select(x => x.AssetPair).OrderBy(x => x).ToList();

            await _tickPriceManager.UpdateRunners(rows, columns);
        }
    }
}
