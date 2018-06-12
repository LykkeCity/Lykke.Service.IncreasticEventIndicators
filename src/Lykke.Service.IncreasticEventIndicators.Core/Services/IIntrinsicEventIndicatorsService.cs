using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;

namespace Lykke.Service.IncreasticEventIndicators.Core.Services
{
    public interface IIntrinsicEventIndicatorsService
    {
        Task AddColumn(IIntrinsicEventIndicatorsColumn column);
        Task RemoveColumn(string columnId);

        Task AddAssetPair(IIntrinsicEventIndicatorsRow row);
        Task RemoveAssetPair(string rowId);

        Task<IntrinsicEventIndicators> GetData();

        Task<IDictionary<string, IList<IRunnerState>>> GetRunnersStates();
    }
}
