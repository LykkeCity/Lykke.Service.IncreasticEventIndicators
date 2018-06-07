using System;

namespace Lykke.Service.IncreasticEventIndicators.Core.Domain.Model
{
    public interface ITickPrice
    {
        string Asset { get; set; }
        DateTime Timestamp { get; set; }

        decimal Ask { get; set; }
        decimal Bid { get; set; }
    }
}
