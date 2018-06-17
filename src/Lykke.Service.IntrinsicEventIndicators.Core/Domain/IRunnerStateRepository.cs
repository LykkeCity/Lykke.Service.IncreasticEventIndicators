using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public interface IRunnerStateRepository
    {
        Task<IReadOnlyList<IRunnerState>> GetState();
        Task SaveState(IReadOnlyList<IRunnerState> state);
        Task CleanOldItems(IEnumerable<string> exchangeAssetPairs, IEnumerable<decimal> deltas);
    }
}
