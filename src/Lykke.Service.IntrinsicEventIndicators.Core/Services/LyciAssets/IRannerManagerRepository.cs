using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Services.LyciAssets
{
    public interface IRannerManagerRepository
    {
        Task<List<RunnerState>> GetAllAsync();
        Task SaveAllAsync(IEnumerable<IRunnerState> runners);
    }
}
