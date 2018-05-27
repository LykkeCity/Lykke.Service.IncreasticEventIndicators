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
                }
                else
                {
                    throw new Exception($"Monitor not entered for {Constants.LockTimeout} in {nameof(TickPriceManager)}");
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorAsync(nameof(TickPriceManager), nameof(UpdateRunners), ex).GetAwaiter().GetResult();
            }
            finally
            {
                if (entered)
                {
                    Monitor.Exit(_syncRoot);
                }
            }

            return Task.CompletedTask;
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
                }
                else
                {
                    throw new Exception($"Monitor not entered for {Constants.LockTimeout} in {nameof(TickPriceManager)}");
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorAsync(nameof(TickPriceManager), nameof(Handle), ex).GetAwaiter().GetResult();
            }
            finally
            {
                if (entered)
                {
                    Monitor.Exit(_syncRoot);
                }
            }

            return Task.CompletedTask;
        }

        private void UpdateRunnersInternal(IList<string> assetPairs, IList<decimal> deltas)
        {
            _assetPairs =
                new ConcurrentDictionary<string, object>(assetPairs.Select(x =>
                    new KeyValuePair<string, object>(x, null)));
            _deltas = new ConcurrentDictionary<decimal, object>(deltas.Select(x =>
                new KeyValuePair<decimal, object>(x, null)));

            foreach (var assetPair in assetPairs)
            {
                foreach (var delta in deltas)
                {
                    var key = GetRunnersKey(assetPair, delta);
                    if (!_runners.ContainsKey(key))
                    {
                        var deltaDouble = (double)delta;
                        _runners.TryAdd(key, new Runner(deltaDouble, deltaDouble, deltaDouble, deltaDouble, 0));
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
            if (_assetPairs.ContainsKey(tickPrice.Asset))
            {
                foreach (var delta in _deltas.Keys)
                {
                    var key = GetRunnersKey(tickPrice.Asset, delta);
                    if (_runners.ContainsKey(key))
                    {
                        _runners[key].Run(tickPrice);
                    }
                }
            }
        }

        private static string GetRunnersKey(string assetPair, decimal delta)
        {
            return $"{assetPair}_{delta}";
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
