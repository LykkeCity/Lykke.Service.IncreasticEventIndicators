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
        private readonly ILog _log;

        private ConcurrentDictionary<string, object> _assetPairs = new ConcurrentDictionary<string, object>();
        private ConcurrentDictionary<decimal, object> _deltas = new ConcurrentDictionary<decimal, object>();

        private readonly ConcurrentDictionary<string, Runner> _runners = new ConcurrentDictionary<string, Runner>();

        private readonly object _syncRoot = new object();

        public TickPriceManager(ILog log)
        {
            _log = log;
        }

        public Task UpdateRunners(IList<string> assetPairs, IList<decimal> deltas)
        {
            var entered = false;
            try
            {
                Monitor.TryEnter(_syncRoot, Constants.LockTimeout, ref entered);
                if (entered)
                {
                    UpdateRunnersInternal(assetPairs, deltas);
                    return Task.CompletedTask;
                }
                else
                {
                    throw new Exception($"Monitor not entered for {Constants.LockTimeout} in {nameof(TickPriceManager)}");
                }
            }
            finally
            {
                if (entered)
                {
                    Monitor.Exit(_syncRoot);
                }
            }            
        }

        public Task Handle(ITickPrice tickPrice)
        {
            var entered = false;
            try
            {
                Monitor.TryEnter(_syncRoot, Constants.LockTimeout, ref entered);
                if (entered)
                {
                    HandleInternal(tickPrice);
                    return Task.CompletedTask;
                }
                else
                {
                    throw new Exception($"Monitor not entered for {Constants.LockTimeout} in {nameof(TickPriceManager)}");
                }
            }
            finally
            {
                if (entered)
                {
                    Monitor.Exit(_syncRoot);
                }
            }            
        }

        public Task<decimal[][]> GetIntrinsicEventIndicators(IList<string> assetPairs, IList<decimal> deltas)
        {
            var entered = false;
            try
            {
                Monitor.TryEnter(_syncRoot, Constants.LockTimeout, ref entered);
                if (entered)
                {
                    var data = GetIntrinsicEventIndicatorsInternal(assetPairs, deltas);
                    return Task.FromResult(data);
                }
                else
                {
                    throw new Exception($"Monitor not entered for {Constants.LockTimeout} in {nameof(TickPriceManager)}");
                }
            }
            finally
            {
                if (entered)
                {
                    Monitor.Exit(_syncRoot);
                }
            }
        }

        public Task<IDictionary<string, IList<IRunnerState>>> GetRunnersStates()
        {
            var entered = false;
            try
            {
                Monitor.TryEnter(_syncRoot, Constants.LockTimeout, ref entered);
                if (entered)
                {
                    return Task.FromResult(GetRunnersStatesInternal());
                }
                else
                {
                    throw new Exception($"Monitor not entered for {Constants.LockTimeout} in {nameof(TickPriceManager)}");
                }
            }
            finally
            {
                if (entered)
                {
                    Monitor.Exit(_syncRoot);
                }
            }
        }

        private void UpdateRunnersInternal(IList<string> assetPairs, IList<decimal> deltas)
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
                        _runners.TryAdd(key, new Runner(delta));
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
