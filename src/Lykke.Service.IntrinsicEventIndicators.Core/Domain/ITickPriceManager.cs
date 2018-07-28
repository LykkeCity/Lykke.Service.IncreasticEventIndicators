using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public interface ITickPriceManager
    {
        Task UpdateRunners(IList<string> exchangeAssetPairs, IList<decimal> deltas);
        Task<IntrinsicEventIndicatorsData> GetIntrinsicEventIndicators(IList<string> exchangeAssetPairs, IList<decimal> deltas);
        Task<IDictionary<string, IList<IRunnerState>>> GetRunnersStates();
    }

    public class IntrinsicEventIndicatorsData
    {
        public decimal[][] Data { get; set; }
        public TimeSpan?[][] TimesFromDc { get; set; }
    }
}
