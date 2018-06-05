using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.IncreasticEventIndicators.Core;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;
using Lykke.Service.IncreasticEventIndicators.Core.Services.Exchanges;

namespace Lykke.Service.IncreasticEventIndicators.Services.Exchanges
{
    public class TickPriceManager : ITickPriceManager, ILykkeTickPriceHandler
    {
        private static readonly TimeSpan SavePeriod = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan CleanPeriod = TimeSpan.FromHours(1);

        private readonly ILog _log;
        private readonly IRunnerStateRepository _runnerStateRepository;

        private ConcurrentDictionary<string, object> _assetPairs = new ConcurrentDictionary<string, object>();
        private ConcurrentDictionary<decimal, object> _deltas = new ConcurrentDictionary<decimal, object>();

        private ConcurrentDictionary<string, Runner> _runners = new ConcurrentDictionary<string, Runner>();

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private bool _initialized;
        private readonly Timer _saveStateTimer;
        private readonly Timer _cleanStateTimer;

        public TickPriceManager(ILog log, IRunnerStateRepository runnerStateRepository)
        {
            _log = log;
            _runnerStateRepository = runnerStateRepository;
            _saveStateTimer = new Timer(OnTimer, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            _cleanStateTimer = new Timer(OnCleanTimer, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        public async Task UpdateRunners(IList<string> assetPairs, IList<decimal> deltas)
        {
            await EnsureInitialized();

            var lockTaken = false;
            try
            {
                lockTaken = await _semaphore.WaitAsync(Constants.LockTimeout);
                if (lockTaken)
                {
                    UpdateRunnersInternal(assetPairs, deltas);
                }
                else
                {
                    throw new Exception("Deadlock occured");
                }
            }
            finally
            {
                if (lockTaken)
                {
                    _semaphore.Release();
                }
            }            
        }

        public async Task Handle(ITickPrice tickPrice)
        {
            await EnsureInitialized();

            var lockTaken = false;
            try
            {
                lockTaken = await _semaphore.WaitAsync(Constants.LockTimeout);
                if (lockTaken)
                {
                    HandleInternal(tickPrice);
                }
                else
                {
                    throw new Exception("Deadlock occured");
                }
            }
            finally
            {
                if (lockTaken)
                {
                    _semaphore.Release();
                }
            }
        }

        public Task<decimal[][]> GetIntrinsicEventIndicators(IList<string> assetPairs, IList<decimal> deltas)
        {
            var lockTaken = false;
            try
            {
                lockTaken = _semaphore.Wait(Constants.LockTimeout);
                if (lockTaken)
                {
                    return Task.FromResult(GetIntrinsicEventIndicatorsInternal(assetPairs, deltas));
                }
                else
                {
                    throw new Exception("Deadlock occured");
                }
            }
            finally
            {
                if (lockTaken)
                {
                    _semaphore.Release();
                }
            }
        }

        public Task<IDictionary<string, IList<IRunnerState>>> GetRunnersStates()
        {
            var lockTaken = false;
            try
            {
                lockTaken = _semaphore.Wait(Constants.LockTimeout);
                if (lockTaken)
                {
                    return Task.FromResult(GetRunnersStatesInternal());
                }
                else
                {
                    throw new Exception("Deadlock occured");
                }
            }
            finally
            {
                if (lockTaken)
                {
                    _semaphore.Release();
                }
            }
        }

        private async Task EnsureInitialized()
        {
            if (_initialized) return;

            var lockTaken = false;
            try
            {
                lockTaken = await _semaphore.WaitAsync(Constants.LockTimeout);
                if (lockTaken)
                {
                    if (_initialized) return;

                    await EnsureInitializedInternal();
                }
                else
                {
                    throw new Exception("Deadlock occured");
                }
            }
            finally
            {
                if (lockTaken)
                {
                    _semaphore.Release();
                }
            }
        }

        private async Task EnsureInitializedInternal()
        {
            var runnerStatesEntities = await _runnerStateRepository.GetState();

            _runners = new ConcurrentDictionary<string, Runner>();
            foreach (var runnerStateEntity in runnerStatesEntities)
            {
                var runnerState = new RunnerState(runnerStateEntity.Event, runnerStateEntity.Extreme,
                    runnerStateEntity.ExpectedDcLevel, runnerStateEntity.ExpectedOsLevel, runnerStateEntity.Reference,
                    runnerStateEntity.ExpectedDirectionalChange, runnerStateEntity.DirectionalChangePrice,
                    runnerStateEntity.Delta, runnerStateEntity.AssetPair);
                _runners.TryAdd(runnerState.AssetPair, new Runner(runnerStateEntity.Delta, runnerStateEntity.AssetPair, runnerState));
            }

            _saveStateTimer.Change(SavePeriod, Timeout.InfiniteTimeSpan);

            _initialized = true;
        }

        private void UpdateRunnersInternal(IEnumerable<string> assetPairs, IEnumerable<decimal> deltas)
        {
            _assetPairs =
                new ConcurrentDictionary<string, object>(assetPairs.Select(x =>
                    new KeyValuePair<string, object>(x.ToUpperInvariant(), null)));
            _deltas = new ConcurrentDictionary<decimal, object>(deltas.Select(x =>
                new KeyValuePair<decimal, object>(x, null)));

            foreach (var assetPair in _assetPairs.Keys)
            {
                foreach (var delta in _deltas.Keys)
                {
                    var key = GetRunnersKey(assetPair, delta);
                    if (!_runners.ContainsKey(key))
                    {
                        _runners.TryAdd(key, new Runner(delta, assetPair));
                    }
                }
            }

            var runnerKeys = _runners.Keys.ToList();
            foreach (var runnerKey in runnerKeys)
            {
                var assetPair = ParseAssetPairFromKey(runnerKey);
                var delta = ParseDeltaFromKey(runnerKey);

                if (!_assetPairs.ContainsKey(assetPair) || !_deltas.ContainsKey(delta))
                {
                    _runners.TryRemove(runnerKey, out _);
                }
            }
        }

        private void HandleInternal(ITickPrice tickPrice)
        {
            var tickPriceAssetPair = tickPrice.Asset.ToUpperInvariant();
            if (_assetPairs.ContainsKey(tickPriceAssetPair))
            {
                foreach (var delta in _deltas.Keys)
                {
                    var key = GetRunnersKey(tickPriceAssetPair, delta);
                    if (_runners.ContainsKey(key))
                    {
                        _runners[key].Run(tickPrice);
                    }
                }
            }
        }

        private decimal[][] GetIntrinsicEventIndicatorsInternal(IList<string> assetPairs, IList<decimal> deltas)
        {
            var data = new decimal[assetPairs.Count][];
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = new decimal[deltas.Count];
            }

            for (var i = 0; i < assetPairs.Count; i++)
            {
                for (var j = 0; j < deltas.Count; j++)
                {
                    var key = GetRunnersKey(assetPairs[i], deltas[j]);
                    if (_runners.ContainsKey(key))
                    {
                        data[i][j] = _runners[key].CalcIntrinsicEventIndicator();
                    }
                }
            }

            return data;
        }

        private IDictionary<string, IList<IRunnerState>> GetRunnersStatesInternal()
        {
            var runnersStates = new Dictionary<string, IList<IRunnerState>>();

            foreach (var runner in _runners)
            {
                var assetPair = ParseAssetPairFromKey(runner.Key);
                if (!runnersStates.ContainsKey(assetPair))
                {
                    runnersStates.Add(assetPair, new List<IRunnerState>());
                }

                runnersStates[assetPair].Add(runner.Value.GetState());
            }

            return runnersStates;
        }

        private void OnTimer(object state)
        {
            SaveState();
            _saveStateTimer.Change(SavePeriod, Timeout.InfiniteTimeSpan);
        }

        public void SaveState()
        {
            if (!_initialized) return;

            var lockTaken = false;
            try
            {
                lockTaken = _semaphore.Wait(Constants.LockTimeout);
                if (lockTaken)
                {
                    SaveStateInternal();
                }
                else
                {
                    throw new Exception("Deadlock occured");
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorAsync(nameof(TickPriceManager), nameof(SaveState), ex).GetAwaiter().GetResult();
            }
            finally
            {
                if (lockTaken)
                {
                    _semaphore.Release();
                }
            }
        }

        private void SaveStateInternal()
        {
            var runnersStates = _runners.Values.Where(x => x.IsStateChanged).ToList();
            if (runnersStates.Count == 0) return;

            _runnerStateRepository.SaveState(runnersStates
                .Select(x => x.GetState())
                .ToArray());

            runnersStates.ForEach(x => x.SaveState());
        }

        private void OnCleanTimer(object state)
        {
            CleanState();
            _cleanStateTimer.Change(CleanPeriod, Timeout.InfiniteTimeSpan);
        }

        public void CleanState()
        {
            if (!_initialized) return;

            var lockTaken = false;
            try
            {
                lockTaken = _semaphore.Wait(Constants.LockTimeout);
                if (lockTaken)
                {
                    CleanStateInternal();
                }
                else
                {
                    throw new Exception("Deadlock occured");
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorAsync(nameof(TickPriceManager), nameof(CleanState), ex).GetAwaiter().GetResult();
            }
            finally
            {
                if (lockTaken)
                {
                    _semaphore.Release();
                }
            }
        }

        private void CleanStateInternal()
        {
            _runnerStateRepository.CleanOldItems(_assetPairs.Keys, _deltas.Keys);
        }

        private static string GetRunnersKey(string assetPair, decimal delta)
        {
            return $"{assetPair.ToUpperInvariant()}_{delta}";
        }

        private static string ParseAssetPairFromKey(string key)
        {
            return key.Split('_')[0];
        }

        private static decimal ParseDeltaFromKey(string key)
        {
            return decimal.Parse(key.Split('_')[1]);
        }
    }
}
