using System;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;

namespace Lykke.Service.IncreasticEventIndicators.Core.Domain
{
    public class Runner
    {
        public decimal Delta { get; private set; }

        public ExpectedDirectionalChange ExpectedDirectionalChange
        {
            get => _state.ExpectedDirectionalChange;
            private set => _state.ExpectedDirectionalChange = value;
        }

        public bool IsStateChanged => _state.IsChanged;

        public decimal ExpectedDcLevel => _state.ExpectedDcLevel;
        public int Event => _state.Event;

        private bool _initialized;
        private readonly RunnerState _state;

        public Runner(decimal delta)
        {
            ResetDeltas(delta);

            _state = new RunnerState();
            ExpectedDirectionalChange = ExpectedDirectionalChange.Upward;
            _initialized = false;
        }

        public void ResetDeltas(decimal delta)
        {
            Delta = delta;
        }

        public void Run(ITickPrice tickPrice) // TODO: probably it should return Tuple<ExpectedEvent, ExpectedLevel>
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

            if (ExpectedDirectionalChange == ExpectedDirectionalChange.Upward)
            {
                if (tickPrice.Bid >= _state.ExpectedDcLevel)
                {
                    ExpectedDirectionalChange = ExpectedDirectionalChange.Downward;
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
                    if (tickPrice.Ask <= _state.ExpectedOsLevel) // deltaUp, it is correct. This is the difference of the trading algo.
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
                    ExpectedDirectionalChange = ExpectedDirectionalChange.Upward;
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
                    if (tickPrice.Bid >= _state.ExpectedOsLevel) // deltaDown, it is correct. This is the difference of the trading algo.
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
            if (_state.Extreme == 0 || (decimal)Delta == 0)
            {
                return 0;
            }

            var indicator = Math.Abs((_state.Extreme - _state.DirectionalChangePrice) / _state.Extreme / (decimal) Delta);
            return indicator;
        }

        private decimal CalcExpectedDClevel()
        {
            if (ExpectedDirectionalChange == ExpectedDirectionalChange.Upward)
            {
                return (decimal)Math.Exp(Math.Log(decimal.ToDouble(_state.Extreme)) + (double)Delta);
            }
            else
            {
                return (decimal)Math.Exp(Math.Log(decimal.ToDouble(_state.Extreme)) - (double)Delta);
            }
        }

        private decimal CalcExpectedOSlevel()
        {
            if (ExpectedDirectionalChange == ExpectedDirectionalChange.Upward)
            {
                return (decimal)Math.Exp(Math.Log(decimal.ToDouble(_state.Reference)) - (double)Delta);
            }
            else
            {
                return (decimal)Math.Exp(Math.Log(decimal.ToDouble(_state.Reference)) + (double)Delta);
            }
        }

        public decimal ExpectedUpperIE => Math.Max(_state.ExpectedDcLevel, _state.ExpectedOsLevel);

        public decimal ExpectedLowerIE => Math.Min(_state.ExpectedDcLevel, _state.ExpectedOsLevel);

        public IntrinsicEvent UpperIe => _state.ExpectedDcLevel > _state.ExpectedOsLevel
            ? IntrinsicEvent.DirectionalChange
            : IntrinsicEvent.Overshoot;

        public IntrinsicEvent LowerIe => _state.ExpectedDcLevel < _state.ExpectedOsLevel
            ? IntrinsicEvent.DirectionalChange
            : IntrinsicEvent.Overshoot;

        public IRunnerState SaveState()
        {
            _state.IsChanged = false;
            return (IRunnerState)_state.Clone();
        }

        public IRunnerState GetState()
        {
            return (IRunnerState)_state.Clone();
        }
    }
}
