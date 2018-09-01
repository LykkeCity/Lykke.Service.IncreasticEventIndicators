using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain.Model;
using Lykke.Service.IntrinsicEventIndicators.Core.Services;

namespace Lykke.Service.IntrinsicEventIndicators.Services
{
    public abstract class IntrinsicEventIndicatorsService : IIntrinsicEventIndicatorsService
    {
        private readonly IIntrinsicEventIndicatorsRepository _repo;
        private readonly IMatrixHistoryRepository _matrixHistoryRepo;
        private readonly IEventHistoryRepository _eventHistoryRepo;
        private readonly ILog _log;
        private readonly ITickPriceManager _tickPriceManager;

        private bool _initialized;

        protected IntrinsicEventIndicatorsService(IIntrinsicEventIndicatorsRepository repo,
            IMatrixHistoryRepository matrixHistoryRepo, IEventHistoryRepository eventHistoryRepo,
            ITickPriceManager tickPriceManager, ILogFactory logFactory)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _matrixHistoryRepo = matrixHistoryRepo ?? throw new ArgumentNullException(nameof(matrixHistoryRepo));
            _eventHistoryRepo = eventHistoryRepo ?? throw new ArgumentNullException(nameof(eventHistoryRepo));
            _log = logFactory.CreateLog(this);
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

        public async Task EditAssetPair(IIntrinsicEventIndicatorsRow row)
        {
            EnsureInitialized();

            SetToReinitialize();
            await _repo.EditAssetPairAsync(row);
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

        public async Task<IList<DateTime>> GetMatrixHistoryStamps(DateTime date)
        {
            return await _matrixHistoryRepo.GetMatrixHistoryStamps(date);
        }

        public async Task<Core.Domain.Model.IntrinsicEventIndicators> GetMatrixHistoryData(DateTime date)
        {
            var data = await _matrixHistoryRepo.GetMatrixHistoryData(date);
            return data?.DeserializeJson<Core.Domain.Model.IntrinsicEventIndicators>();
        }

        public Task<IReadOnlyList<IEventHistory>> GetEventHistoryData(DateTime from, DateTime to, string exchange, string assetPair, decimal? delta)
        {
            return _eventHistoryRepo.GetEventHistoryData(from, to, exchange, assetPair, delta);
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
