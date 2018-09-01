using System;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public interface IEventHistory
    {
        int Event { get; }
        decimal Extreme { get; }
        decimal ExpectedDcLevel { get; }
        decimal ExpectedOsLevel { get; }
        decimal Reference { get; }
        ExpectedDirectionalChange ExpectedDirectionalChange { get; }
        decimal DirectionalChangePrice { get; }
        decimal Delta { get; }
        string AssetPair { get; }
        string Exchange { get; }
        decimal Ask { get; }
        decimal Bid { get; }
        DateTime? TickPriceTimestamp { get; }
        DateTime? DcTimestamp { get; }

        decimal IntrinsicEventIndicator { get; }
    }

    public class EventHistory : IEventHistory
    {
        public int Event { get; set; }
        public decimal Extreme { get; set; }
        public decimal ExpectedDcLevel { get; set; }
        public decimal ExpectedOsLevel { get; set; }
        public decimal Reference { get; set; }
        public ExpectedDirectionalChange ExpectedDirectionalChange { get; set; }
        public decimal DirectionalChangePrice { get; set; }
        public decimal Delta { get; set; }
        public string AssetPair { get; set; }
        public string Exchange { get; set; }
        public decimal Ask { get; set; }
        public decimal Bid { get; set; }
        public DateTime? TickPriceTimestamp { get; set; }
        public DateTime? DcTimestamp { get; set; }

        public decimal IntrinsicEventIndicator { get; set; }
    }
}
