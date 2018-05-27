using System;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;

namespace Lykke.Service.IncreasticEventIndicators.Core.Domain
{
    public class Runner
    {
        public int Index { get; }
        public double DeltaUp { get; private set; }
        public double DeltaDown { get; private set; }
        public double DeltaStarUp { get; private set; }
        public double DeltaStarDown { get; private set; }

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

        public Runner(double deltaUp, double deltaDown, double dStarUp, double dStarDown, int index, IRunnerState state = null)
        {
            Index = index;
            ResetDeltas(deltaUp, deltaDown, dStarUp, dStarDown);
            if (state != null)
            {
                _state = new RunnerState(state.Event, state.Extreme, state.ExpectedDcLevel, state.ExpectedOsLevel,
                    state.Reference, state.ExpectedDirectionalChange);
                _initialized = true;
            }
            else
            {
                _state = new RunnerState();
                ExpectedDirectionalChange = ExpectedDirectionalChange.Upward;
                _initialized = false;
            }
        }

        public void ResetDeltas(double deltaUp, double deltaDown, double dStarUp, double dStarDown)
        {
            DeltaUp = deltaUp;
            DeltaDown = deltaDown;
            DeltaStarUp = dStarUp;
            DeltaStarDown = dStarDown;
        }

        public void Run(ITickPrice tickPrice) // TODO: probably it should return Tuple<ExpectedEvent, ExpectedLevel>
        {
            if (!_initialized)
            {
                _state.Extreme = _state.Reference = (tickPrice.Ask + tickPrice.Bid) / 2m;
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
                    _state.Extreme = _state.Reference = tickPrice.Bid;

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
                    _state.Extreme = _state.Reference = tickPrice.Ask;
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

        private decimal CalcExpectedDClevel()
        {
            if (ExpectedDirectionalChange == ExpectedDirectionalChange.Upward)
            {
                return (decimal)Math.Exp(Math.Log(decimal.ToDouble(_state.Extreme)) + DeltaUp);
            }
            else
            {
                return (decimal)Math.Exp(Math.Log(decimal.ToDouble(_state.Extreme)) - DeltaDown);
            }
        }

        private decimal CalcExpectedOSlevel()
        {
            if (ExpectedDirectionalChange == ExpectedDirectionalChange.Upward)
            {
                return (decimal)Math.Exp(Math.Log(decimal.ToDouble(_state.Reference)) - DeltaStarDown);
            }
            else
            {
                return (decimal)Math.Exp(Math.Log(decimal.ToDouble(_state.Reference)) + DeltaStarUp);
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
