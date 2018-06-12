using System;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;

namespace Lykke.Service.IncreasticEventIndicators.Core.Domain
{
    public class Runner
    {
        private bool _initialized;
        private readonly RunnerState _state;

        public bool IsStateChanged => _state.IsChanged;

        public Runner(decimal delta, string assetPair, string exchange)
        {
            _state = new RunnerState(delta, assetPair, exchange);
            _initialized = false;
        }

        public Runner(RunnerState state)
        {
            _state = state;
            _initialized = true;
        }

        public void Run(ITickPrice tickPrice)
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
