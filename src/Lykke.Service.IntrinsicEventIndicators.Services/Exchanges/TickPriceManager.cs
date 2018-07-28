using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.ExchangeAdapter.Contracts;
using Lykke.Service.IntrinsicEventIndicators.Core;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;

namespace Lykke.Service.IntrinsicEventIndicators.Services.Exchanges
{
    public abstract class TickPriceManager : ITickPriceManager, ITickPriceHandler
    {
        private static readonly TimeSpan SavePeriod = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan CleanPeriod = TimeSpan.FromHours(1);

        private readonly ILog _log;
        private readonly IRunnerStateRepository _runnerStateRepository;

        private ConcurrentDictionary<string, object> _exchangeAssetPairs = new ConcurrentDictionary<string, object>();
        private ConcurrentDictionary<decimal, object> _deltas = new ConcurrentDictionary<decimal, object>();
        private ConcurrentDictionary<string, Runner> _runners = new ConcurrentDictionary<string, Runner>();

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private bool _initialized;
        private readonly Timer _saveStateTimer;
        private readonly Timer _cleanStateTimer;

        protected TickPriceManager(ILog log, IRunnerStateRepository runnerStateRepository)
        {
            _log = log;
            _runnerStateRepository = runnerStateRepository;
            _saveStateTimer = new Timer(OnTimer, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            _cleanStateTimer = new Timer(OnCleanTimer, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        public async Task UpdateRunners(IList<string> exchangeAssetPairs, IList<decimal> deltas)
        {
            await EnsureInitialized();

            var lockTaken = false;
            try
            {
                lockTaken = await _semaphore.WaitAsync(Constants.LockTimeout);
                if (lockTaken)
                {
                    UpdateRunnersInternal(exchangeAssetPairs, deltas);
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

        public async Task Handle(TickPrice tickPrice)
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

        public async Task<IntrinsicEventIndicatorsData> GetIntrinsicEventIndicators(IList<string> exchangeAssetPairs, IList<decimal> deltas)
        {
            await EnsureInitialized();

            var lockTaken = false;
            try
            {
                lockTaken = _semaphore.Wait(Constants.LockTimeout);
                if (lockTaken)
                {
                    return GetIntrinsicEventIndicatorsInternal(exchangeAssetPairs, deltas);
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

        public async Task<IDictionary<string, IList<IRunnerState>>> GetRunnersStates()
        {
            await EnsureInitialized();

            var lockTaken = false;
            try
            {
                lockTaken = _semaphore.Wait(Constants.LockTimeout);
                if (lockTaken)
                {
                    return GetRunnersStatesInternal();
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
            _runners = new ConcurrentDictionary<string, Runner>();

            var runnerStatesEntities = await _runnerStateRepository.GetState();
            foreach (var runnerStateEntity in runnerStatesEntities)
            {
                var runnerState = new RunnerState(runnerStateEntity.Event, runnerStateEntity.Extreme,
                    runnerStateEntity.ExpectedDcLevel, runnerStateEntity.ExpectedOsLevel, runnerStateEntity.Reference,
                    runnerStateEntity.ExpectedDirectionalChange, runnerStateEntity.DirectionalChangePrice,
                    runnerStateEntity.Delta, runnerStateEntity.AssetPair, runnerStateEntity.Exchange,
                    runnerStateEntity.Ask, runnerStateEntity.Bid, runnerStateEntity.TickPriceTimestamp,
                    runnerStateEntity.DcTimestamp);

                var runner = new Runner(runnerState, _log);

                var runnersKey = GetRunnersKey(GetExchangeAssetPairKey(runnerState.Exchange, runnerState.AssetPair), runnerState.Delta);
                _runners.TryAdd(runnersKey, runner);
            }

            _saveStateTimer.Change(SavePeriod, Timeout.InfiniteTimeSpan);
            _cleanStateTimer.Change(CleanPeriod, Timeout.InfiniteTimeSpan);

            _initialized = true;
        }

        private void UpdateRunnersInternal(IEnumerable<string> exchangeAssetPairs, IEnumerable<decimal> deltas)
        {
            _exchangeAssetPairs = new ConcurrentDictionary<string, object>(exchangeAssetPairs.Select(x =>
                new KeyValuePair<string, object>(x.ToUpperInvariant(), null)));
            _deltas = new ConcurrentDictionary<decimal, object>(deltas.Select(x =>
                new KeyValuePair<decimal, object>(x, null)));

            foreach (var exchangeAssetPair in _exchangeAssetPairs.Keys)
            {
                foreach (var delta in _deltas.Keys)
                {
                    var runnersKey = GetRunnersKey(exchangeAssetPair, delta);
                    if (!_runners.ContainsKey(runnersKey))
                    {
                        _runners.TryAdd(runnersKey, new Runner(delta, ParseAssetPairFromExchangeAssetPairKey(exchangeAssetPair),
                            ParseExchangeFromExchangeAssetPairKey(exchangeAssetPair), _log));
                    }
                }
            }

            var runnerKeys = _runners.Keys.ToList();
            foreach (var runnerKey in runnerKeys)
            {
                var exchangeAssetPair = ParseExchangeAssetPairFromRunnersKey(runnerKey);
                var delta = ParseDeltaFromRunnersKey(runnerKey);

                if (!_exchangeAssetPairs.ContainsKey(exchangeAssetPair) || !_deltas.ContainsKey(delta))
                {
                    _runners.TryRemove(runnerKey, out _);
                }
            }
        }

        private void HandleInternal(TickPrice tickPrice)
        {
            var exchangeAssetPair = GetExchangeAssetPairKey(tickPrice.Source.ToUpperInvariant(), tickPrice.Asset.ToUpperInvariant());
            if (_exchangeAssetPairs.ContainsKey(exchangeAssetPair))
            {
                foreach (var delta in _deltas.Keys)
                {
                    var runnersKey = GetRunnersKey(exchangeAssetPair, delta);
                    if (_runners.ContainsKey(runnersKey))
                    {
                        _runners[runnersKey].Run(tickPrice);
                    }
                }
            }
        }

        private IntrinsicEventIndicatorsData GetIntrinsicEventIndicatorsInternal(IList<string> exchangeAssetPairs, IList<decimal> deltas)
        {
            var now = DateTime.UtcNow;

            var data = new decimal[exchangeAssetPairs.Count][];
            var timesFromDc = new TimeSpan?[exchangeAssetPairs.Count][];
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = new decimal[deltas.Count];
                timesFromDc[i] = new TimeSpan?[deltas.Count];
            }

            for (var i = 0; i < exchangeAssetPairs.Count; i++)
            {
                for (var j = 0; j < deltas.Count; j++)
                {
                    var key = GetRunnersKey(exchangeAssetPairs[i], deltas[j]);
                    if (_runners.ContainsKey(key))
                    {
                        data[i][j] = _runners[key].CalcIntrinsicEventIndicator();
                        timesFromDc[i][j] = _runners[key].CalcTimeFromDc(now);
                    }
                }
            }

            return new IntrinsicEventIndicatorsData
            {
                Data = data,
                TimesFromDc = timesFromDc
            };
        }

        private IDictionary<string, IList<IRunnerState>> GetRunnersStatesInternal()
        {
            var runnersStates = new Dictionary<string, IList<IRunnerState>>();

            foreach (var runner in _runners)
            {
                var runnerStateKey = ParseRunnersStatesKeyFromRunnersKey(runner.Key);
                if (!runnersStates.ContainsKey(runnerStateKey))
                {
                    runnersStates.Add(runnerStateKey, new List<IRunnerState>());
                }

                runnersStates[runnerStateKey].Add(runner.Value.GetState());
            }

            return runnersStates;
        }

        private void OnTimer(object state)
        {
            SaveState();
            _saveStateTimer.Change(SavePeriod, Timeout.InfiniteTimeSpan);
        }

        private void SaveState()
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

        private void CleanState()
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
            _runnerStateRepository.CleanOldItems(_exchangeAssetPairs.Keys, _deltas.Keys);
        }

        public static string GetExchangeAssetPairKey(string exchangeName, string assetPair)
        {
            return $"{exchangeName.ToUpperInvariant()} {assetPair.ToUpperInvariant()}";
        }

        private static string ParseExchangeFromExchangeAssetPairKey(string key)
        {
            return key.Split(' ')[0];
        }

        protected static string ParseAssetPairFromExchangeAssetPairKey(string key)
        {
            return key.Split(' ')[1];
        }

        private static string GetRunnersKey(string exchangeAssetPair, decimal delta)
        {
            return $"{exchangeAssetPair.ToUpperInvariant()} {delta}";
        }

        protected static string ParseExchangeAssetPairFromRunnersKey(string key)
        {
            return GetExchangeAssetPairKey(key.Split(' ')[0], key.Split(' ')[1]);
        }

        private static decimal ParseDeltaFromRunnersKey(string key)
        {
            return decimal.Parse(key.Split(' ')[2]);
        }

        protected abstract string ParseRunnersStatesKeyFromRunnersKey(string runnersKey);
    }
}
