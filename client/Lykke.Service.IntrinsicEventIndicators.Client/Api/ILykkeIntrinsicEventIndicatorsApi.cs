using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Client.Models;

namespace Lykke.Service.IntrinsicEventIndicators.Client.Api
{
    [PublicAPI]
    public interface ILykkeIntrinsicEventIndicatorsApi
    {
        Task AddDelta(IntrinsicEventIndicatorsColumnPost column);
        Task RemoveDelta(string columnId);
        Task AddAssetPair(IntrinsicEventIndicatorsRowPost row);
        Task EditAssetPair(IntrinsicEventIndicatorsRowEdit row);
        Task RemoveAssetPair(string rowId);
        Task<IntrinsicEventIndicatorsDto> GetData();
        Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStates();
    }
}
