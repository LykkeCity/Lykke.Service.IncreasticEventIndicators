using System;
using Common;
using Common.Log;
using Lykke.Common.ExchangeAdapter.Contracts;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public class Runner
    {
        private readonly ILog _log;

        private bool _initialized;
        private readonly RunnerState _state;

        public bool IsStateChanged => _state.IsChanged;

        public Runner(decimal delta, string assetPair, string exchange, ILog log)
        {
            _log = log;
            _state = new RunnerState(delta, assetPair, exchange);
            _initialized = false;
        }

        public Runner(RunnerState state, ILog log)
        {
            _log = log;
            _state = state;
            _initialized = true;
        }

        public void Run(TickPrice tickPrice)
        {
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
                    _state.Extreme = _state.Reference = _state.DirectionalChangePrice = tickPrice.Bid;

                    _state.ExpectedDcLevel = CalcExpectedDClevel();
                    _state.ExpectedOsLevel = CalcExpectedOSlevel();
                    _state.Event = 1;

                    LogIntrinsicEventHistoryAsync((RunnerState)_state.Clone(), tickPrice);

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

                        return;
                    }
                }
            }
            else
            {
                if (tickPrice.Ask <= _state.ExpectedDcLevel)
                {
                    _state.ExpectedDirectionalChange = ExpectedDirectionalChange.Upward;
                    _state.Extreme = _state.Reference = _state.DirectionalChangePrice = tickPrice.Ask;
                    _state.ExpectedDcLevel = CalcExpectedDClevel();
                    _state.ExpectedOsLevel = CalcExpectedOSlevel();
                    _state.Event = -1;

                    LogIntrinsicEventHistoryAsync((RunnerState)_state.Clone(), tickPrice);

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

            var indicator = Math.Abs((_state.Extreme - _state.DirectionalChangePrice) / _state.Extreme / _state.Delta);

            if (_state.ExpectedDirectionalChange == ExpectedDirectionalChange.Upward)
            {
                indicator *= -1;
            }

            return Math.Round(indicator, 2);
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

            _log.WriteInfoAsync(nameof(Runner), nameof(LogIntrinsicEventHistoryAsync), data.ToJson(),
                "Intrinsic Event detected").GetAwaiter().GetResult();
        }

        public void SaveState()
        {
            _state.IsChanged = false;
        }

        public IRunnerState GetState()
        {
            return (IRunnerState)_state.Clone();
        }
    }
}
