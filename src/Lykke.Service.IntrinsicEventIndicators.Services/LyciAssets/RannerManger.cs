using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.Common.ExchangeAdapter.Contracts;
using Lykke.Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.LyciAssets;

namespace Lykke.Service.IntrinsicEventIndicators.Services.LyciAssets
{
    public class RannerManger: IStartable, IStopable
    {
        private readonly Dictionary<string, List<RunnerLyci>> _runners = new Dictionary<string, List<RunnerLyci>>();
        private readonly IRannerManagerRepository _managerRepository;
        private readonly IPriceManager _priceManager;
        private readonly IIndecatorListSender _indecatorListSender;
        private TimerTrigger _timer;
        private long _iteration = 0;
        private ILog _log;

        public RannerManger(IRannerManagerRepository managerRepository, IPriceManager priceManager, ILogFactory logFactory, IIndecatorListSender indecatorListSender)
        {
            _log = logFactory.CreateLog(this);
            _managerRepository = managerRepository;
            _priceManager = priceManager;
            _indecatorListSender = indecatorListSender;
            _timer = new TimerTrigger(nameof(RannerManger), TimeSpan.FromSeconds(1), logFactory, DoTimer);
        }

        private async Task DoTimer(ITimerTrigger timer, TimerTriggeredHandlerArgs args, CancellationToken cancellationtoken)
        {
            _iteration++;

            if (_iteration < 11)
                return;

            var prices = _priceManager.GetPrices();

            foreach (var price in prices)
            {
                if (!_runners.ContainsKey(price.Asset))
                {
                    _runners[price.Asset] = new List<RunnerLyci>();
                    _runners[price.Asset].Add(new RunnerLyci(0.001m, price.Asset, _log));
                    _runners[price.Asset].Add(new RunnerLyci(0.002m, price.Asset, _log));
                    _runners[price.Asset].Add(new RunnerLyci(0.005m, price.Asset, _log));
                    _runners[price.Asset].Add(new RunnerLyci(0.01m, price.Asset, _log));
                    _runners[price.Asset].Add(new RunnerLyci(0.02m, price.Asset, _log));
                    _runners[price.Asset].Add(new RunnerLyci(0.04m, price.Asset, _log));
                    _runners[price.Asset].Add(new RunnerLyci(0.08m, price.Asset, _log));
                }

                var tick = new TickPrice()
                {
                    Asset = price.Asset,
                    Timestamp = DateTime.UtcNow,
                    Source = "lyci",
                    Ask = price.AvgMid,
                    Bid = price.AvgMid
                };

                foreach (var runner in _runners[price.Asset]) runner.Run(tick);
            }

            if (_iteration % 10 == 0) await SendStatistic();

            if (_iteration % 60 == 0) SaveAll();
        }

        private async Task SendStatistic()
        {
            try
            {
                var runners = GetAllRunners()
                    .Select(e => new IndecatorModel(e.AssetPair.Replace("LYCIUSD", "LCIUSD"), e.Delta, e.CalcIntrinsicEventIndicator()))
                    .OrderBy(e => e.AssetPair + e.Delta.ToString("#.#"))
                    .ToList();

                var message = new IndecatorList
                {
                    Indicators = runners
                };
                await _indecatorListSender.SendAsync(message);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        private IEnumerable<RunnerLyci> GetAllRunners()
        {
            foreach (var runnerList in _runners)
            foreach (var runner in runnerList.Value) yield return runner;
        }

        private void SaveAll()
        {
            var stateList = GetAllRunners().Select(e => e.GetState()).ToList();

            _managerRepository.SaveAllAsync(stateList).GetAwaiter().GetResult();
        }

        public void Start()
        {
            var stateList = _managerRepository.GetAllAsync().GetAwaiter().GetResult();
            foreach (var state in stateList)
            {
                var ranner = new RunnerLyci(state, _log);
                if (!_runners.ContainsKey(state.AssetPair))
                    _runners[state.AssetPair] = new List<RunnerLyci>();
                _runners[state.AssetPair].Add(ranner);
            }

            _timer.Start();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        public void Stop()
        {
            _timer.Stop();
            SaveAll();
        }
    }
}
