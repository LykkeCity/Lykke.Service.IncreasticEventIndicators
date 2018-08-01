using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain.Model;
using Lykke.Service.IntrinsicEventIndicators.Core.Services;

namespace Lykke.Service.IntrinsicEventIndicators.Services
{
    public abstract class IntrinsicEventIndicatorsService : IIntrinsicEventIndicatorsService
    {
        private readonly IIntrinsicEventIndicatorsRepository _repo;
        private readonly ILog _log;
        private readonly ITickPriceManager _tickPriceManager;

        private bool _initialized;

        protected IntrinsicEventIndicatorsService(IIntrinsicEventIndicatorsRepository repo,
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

            SetToReinitialize();
            await _repo.AddColumnAsync(column);
            await UpdateMetadataAndRunners();
        }

        public async Task RemoveColumn(string columnId)
        {
            EnsureInitialized();

            SetToReinitialize();
            await _repo.RemoveColumnAsync(columnId);
            await UpdateMetadataAndRunners();
        }

        public async Task AddAssetPair(IIntrinsicEventIndicatorsRow row)
        {
            EnsureInitialized();

            SetToReinitialize();
            await _repo.AddAssetPairAsync(row);
            await UpdateMetadataAndRunners();
        }

        public async Task RemoveAssetPair(string rowId)
        {
            EnsureInitialized();

            SetToReinitialize();
            await _repo.RemoveAssetPairAsync(rowId);
            await UpdateMetadataAndRunners();
        }

        public async Task<Core.Domain.Model.IntrinsicEventIndicators> GetData()
        {
            EnsureInitialized();

            return await _tickPriceManager.GetIntrinsicEventIndicators();
        }

        public Task<IDictionary<string, IList<IRunnerState>>> GetRunnersStates()
        {
            EnsureInitialized();

            return _tickPriceManager.GetRunnersStates();
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;

            var task = Task.Run(UpdateMetadataAndRunners);
            Task.WaitAll(task);

            _initialized = true;
        }

        private void SetToReinitialize()
        {
            _initialized = false;
        }

        private async Task UpdateMetadataAndRunners()
        {            
            await _tickPriceManager.UpdateMetadataAndRunners();
        }
    }
}
