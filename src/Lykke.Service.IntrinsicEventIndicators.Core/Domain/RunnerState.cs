using System;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public class RunnerState : IRunnerState, ICloneable
    {
        private int _event;
        private decimal _extreme;
        private decimal _expectedDcLevel;
        private decimal _expectedOsLevel;
        private decimal _reference;
        private ExpectedDirectionalChange _expectedDirectionalChange;

        private decimal _directionalChangePrice;

        public int Event
        {
            get => _event;
            set
            {
                if (_event == value) return;
                _event = value;
                IsChanged = true;
            }
        }

        public decimal Extreme
        {
            get => _extreme;
            set
            {
                if (_extreme == value) return;
                _extreme = value;
                IsChanged = true;
            }
        }

        public decimal ExpectedDcLevel
        {
            get => _expectedDcLevel;
            set
            {
                if (_expectedDcLevel == value) return;
                _expectedDcLevel = value;
                IsChanged = true;
            }
        }

        public decimal ExpectedOsLevel
        {
            get => _expectedOsLevel;
            set
            {
                if (_expectedOsLevel == value) return;
                _expectedOsLevel = value;
                IsChanged = true;
            }
        }

        public decimal Reference
        {
            get => _reference;
            set
            {
                if (_reference == value) return;
                _reference = value;
                IsChanged = true;
            }
        }

        public ExpectedDirectionalChange ExpectedDirectionalChange
        {
            get => _expectedDirectionalChange;
            set
            {
                if (_expectedDirectionalChange == value) return;
                _expectedDirectionalChange = value;
                IsChanged = true;
            }
        }

        public decimal DirectionalChangePrice
        {
            get => _directionalChangePrice;
            set
            {
                if (_directionalChangePrice == value) return;
                _directionalChangePrice = value;
                IsChanged = true;
            }
        }

        public decimal Delta { get; }

        public string AssetPair { get; }

        public string Exchange { get; }

        public bool IsChanged { get; set; }

        public RunnerState(decimal delta, string assetPair, string exchange)
        {
            Delta = delta;
            AssetPair = assetPair;
            Exchange = exchange;
        }

        public RunnerState(int @event, decimal extreme, decimal expectedDcLevel, decimal expectedOsLevel,
            decimal reference, ExpectedDirectionalChange expectedDirectionalChange, decimal directionalChangePrice, decimal delta,
            string assetPair, string exchange)
        {
            _event = @event;
            _extreme = extreme;
            _expectedDcLevel = expectedDcLevel;
            _expectedOsLevel = expectedOsLevel;
            _reference = reference;
            _expectedDirectionalChange = expectedDirectionalChange;
            _directionalChangePrice = directionalChangePrice;
            Delta = delta;
            AssetPair = assetPair;
            Exchange = exchange;
        }

        public object Clone()
        {
            return new RunnerState(_event, _extreme, _expectedDcLevel, _expectedOsLevel, _reference,
                _expectedDirectionalChange, _directionalChangePrice, Delta, AssetPair, Exchange);
        }
    }
}
