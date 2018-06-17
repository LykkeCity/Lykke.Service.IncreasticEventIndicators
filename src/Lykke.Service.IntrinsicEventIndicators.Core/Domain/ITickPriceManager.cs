using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public interface ITickPriceManager
    {
        Task UpdateRunners(IList<string> exchangeAssetPairs, IList<decimal> deltas);
        Task<decimal[][]> GetIntrinsicEventIndicators(IList<string> exchangeAssetPairs, IList<decimal> deltas);
        Task<IDictionary<string, IList<IRunnerState>>> GetRunnersStates();
    }
}
