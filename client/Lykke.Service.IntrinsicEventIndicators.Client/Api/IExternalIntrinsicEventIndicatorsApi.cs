using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Client.Models;

namespace Lykke.Service.IntrinsicEventIndicators.Client.Api
{
    [PublicAPI]
    public interface IExternalIntrinsicEventIndicatorsApi
    {
        Task AddDeltaExternal(IntrinsicEventIndicatorsColumnPost column);
        Task RemoveDeltaExternal(string columnId);
        Task AddAssetPairExternal(IntrinsicEventIndicatorsRowPost row);
        Task EditAssetPairExternal(IntrinsicEventIndicatorsRowEdit row);
        Task RemoveAssetPairExternal(string rowId);
        Task<IntrinsicEventIndicatorsDto> GetDataExternal();
        Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesExternal();
    }
}
