using System;
using Common;
using Common.Log;
using Lykke.Common.ExchangeAdapter.Contracts;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public class Runner
    {
        private readonly IEventHistoryRepository _eventHistoryRepository;
        private readonly ILog _log;

        private bool _initialized;
        private readonly RunnerState _state;

        public bool IsStateChanged => _state.IsChanged;

        public Runner(decimal delta, string assetPair, string exchange,
            IEventHistoryRepository eventHistoryRepository, ILog log)
        {
            _eventHistoryRepository = eventHistoryRepository;
            _log = log;
            _state = new RunnerState(delta, assetPair, exchange);
            _initialized = false;
        }

        public Runner(RunnerState state, IEventHistoryRepository eventHistoryRepository, ILog log)
        {
            _eventHistoryRepository = eventHistoryRepository;
            _log = log;
            _state = state;
            _initialized = true;
        }

        public void Run(TickPrice tickPrice)
        {
            _state.Ask = tickPrice.Ask;
            _state.Bid = tickPrice.Bid;
            _state.TickPriceTimestamp = tickPrice.Timestamp;

            if (!_initialized)
            {
                _state.Extreme = _state.Reference = _state.DirectionalChangePrice = (tickPrice.Ask + tickPrice.Bid) / 2m;
                _state.ExpectedDcLevel = CalcExpectedDClevel();
                _state.ExpectedOsLevel = CalcExpectedOSlevel();
                _state.Event = 0;

                _initialized = true;
                return;
            }

            if (_state.ExpectedDirectionalChange == ExpectedDirectionalChange.Upward)
            {
                if (tickPrice.Bid >= _state.ExpectedDcLevel)
                {                    
                    _state.ExpectedDirectionalChange = ExpectedDirectionalChange.Downward;
                    _state.DcTimestamp = DateTime.UtcNow;
                    _state.Extreme = _state.Reference = _state.DirectionalChangePrice = tickPrice.Bid;

                    _state.ExpectedDcLevel = CalcExpectedDClevel();
                    _state.ExpectedOsLevel = CalcExpectedOSlevel();
                    _state.Event = 1;

                    LogIntrinsicEventHistoryAsync((RunnerState)_state.Clone(), tickPrice);
                    SaveEventHistoryInternal((RunnerState) _state.Clone());

                    return;
                }

                if (tickPrice.Ask < _state.Extreme)
                {
                    _state.Extreme = tickPrice.Ask;
                    _state.ExpectedDcLevel = CalcExpectedDClevel();
                    if (tickPrice.Ask <= _state.ExpectedOsLevel)
                    {
                        _state.Reference = _state.Extreme;
                        _state.ExpectedOsLevel = CalcExpectedOSlevel();
                        _state.Event = -2;

                        LogIntrinsicEventHistoryAsync((RunnerState)_state.Clone(), tickPrice);
                        SaveEventHistoryInternal((RunnerState)_state.Clone());

                        return;
                    }
                }
            }
            else
            {
                if (tickPrice.Ask <= _state.ExpectedDcLevel)
                {
                    _state.ExpectedDirectionalChange = ExpectedDirectionalChange.Upward;
                    _state.DcTimestamp = DateTime.UtcNow;
                    _state.Extreme = _state.Reference = _state.DirectionalChangePrice = tickPrice.Ask;
                    _state.ExpectedDcLevel = CalcExpectedDClevel();
                    _state.ExpectedOsLevel = CalcExpectedOSlevel();
                    _state.Event = -1;

                    LogIntrinsicEventHistoryAsync((RunnerState)_state.Clone(), tickPrice);
                    SaveEventHistoryInternal((RunnerState)_state.Clone());

                    return;
                }

                if (tickPrice.Bid > _state.Extreme)
                {
                    _state.Extreme = tickPrice.Bid;
                    _state.ExpectedDcLevel = CalcExpectedDClevel();
                    if (tickPrice.Bid >= _state.ExpectedOsLevel)
                    {
                        _state.Reference = _state.Extreme;
                        _state.ExpectedOsLevel = CalcExpectedOSlevel();
                        _state.Event = 2;

                        LogIntrinsicEventHistoryAsync((RunnerState)_state.Clone(), tickPrice);
                        SaveEventHistoryInternal((RunnerState)_state.Clone());

                        return;
                    }
                }
            }

            _state.Event = 0;
        }

        public decimal CalcIntrinsicEventIndicator()
        {
            if (_state.Extreme == 0 || _state.Delta == 0)
            {
                return 0;
            }

            var indicator = Math.Abs((_state.Extreme - _state.DirectionalChangePrice) / _state.DirectionalChangePrice / _state.Delta);

            if (_state.ExpectedDirectionalChange == ExpectedDirectionalChange.Upward)
            {
                indicator *= -1;
            }

            return Math.Round(indicator, 2);
        }

        public TimeSpan? CalcTimeFromDc(DateTime now)
        {
            return now - _state.DcTimestamp;
        }

        private decimal CalcExpectedDClevel()
        {
            if (_state.ExpectedDirectionalChange == ExpectedDirectionalChange.Upward)
            {
                return (decimal)Math.Round(Math.Exp(Math.Log(decimal.ToDouble(_state.Extreme)) + (double)_state.Delta), 2);
            }
            else
            {
                return (decimal)Math.Round(Math.Exp(Math.Log(decimal.ToDouble(_state.Extreme)) - (double)_state.Delta), 2);
            }
        }

        private decimal CalcExpectedOSlevel()
        {
            if (_state.ExpectedDirectionalChange == ExpectedDirectionalChange.Upward)
            {
                return (decimal)Math.Round(Math.Exp(Math.Log(decimal.ToDouble(_state.Reference)) - (double)_state.Delta), 2);
            }
            else
            {
                return (decimal)Math.Round(Math.Exp(Math.Log(decimal.ToDouble(_state.Reference)) + (double)_state.Delta), 2);
            }
        }

        private void LogIntrinsicEventHistoryAsync(RunnerState runnerState, TickPrice tickPrice)
        {
            var data = new
            {
                RunnerState = runnerState,
                TickPrice = tickPrice
            };

            //_log.WriteInfoAsync(nameof(Runner), nameof(LogIntrinsicEventHistoryAsync), data.ToJson(), "Intrinsic Event detected").GetAwaiter().GetResult();
        }

        private void SaveEventHistoryInternal(RunnerState runnerState)
        {
            _eventHistoryRepository.Save(
                new EventHistory
                {
                    Event = runnerState.Event,
                    Extreme = runnerState.Extreme,
                    ExpectedDcLevel = runnerState.ExpectedDcLevel,
                    ExpectedOsLevel = runnerState.ExpectedOsLevel,
                    Reference = runnerState.Reference,
                    ExpectedDirectionalChange = runnerState.ExpectedDirectionalChange,
                    DirectionalChangePrice = runnerState.DirectionalChangePrice,
                    AssetPair = runnerState.AssetPair,
                    Delta = runnerState.Delta,
                    Exchange = runnerState.Exchange,
                    Ask = runnerState.Ask,
                    Bid = runnerState.Bid,
                    TickPriceTimestamp = runnerState.TickPriceTimestamp,
                    DcTimestamp = runnerState.DcTimestamp,
                    IntrinsicEventIndicator = CalcIntrinsicEventIndicator()
                });
        }

        public void SaveState()
        {
            _state.IsChanged = false;
        }

        public string AssetPair => _state.AssetPair;

        public decimal Delta => _state.Delta;

        public IRunnerState GetState()
        {
            return (IRunnerState)_state.Clone();
        }
    }
}
