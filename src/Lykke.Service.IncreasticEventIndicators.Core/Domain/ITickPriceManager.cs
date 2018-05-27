using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.IncreasticEventIndicators.Core.Domain
{
    public interface ITickPriceManager
    {
        Task UpdateRunners(IList<string> assetPairs, IList<decimal> deltas);
        Task<decimal[][]> GetIntrinsicEventIndicators(IList<string> assetPairs, IList<decimal> deltas);
    }
}
