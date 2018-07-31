using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public interface ITickPriceManager
    {
        Task UpdateMetadataAndRunners();
        Task<Model.IntrinsicEventIndicators> GetIntrinsicEventIndicators();
        Task<IDictionary<string, IList<IRunnerState>>> GetRunnersStates();
    }
}
