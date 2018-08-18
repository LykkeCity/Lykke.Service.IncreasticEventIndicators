using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Client.Models;
using Refit;

namespace Lykke.Service.IntrinsicEventIndicators.Client.Api
{
    [PublicAPI]
    public interface ILykkeIntrinsicEventIndicatorsApi
    {
        [Post("/api/lykkeintrinsiceventindicators/delta")]
        Task AddDeltaAsync(IntrinsicEventIndicatorsColumnPost column);
        [Delete("/api/lykkeintrinsiceventindicators/delta")]
        Task RemoveDeltaAsync(string columnId);
        [Post("/api/lykkeintrinsiceventindicators/assetpair")]
        Task AddAssetPairAsync(IntrinsicEventIndicatorsRowPost row);
        [Put("/api/lykkeintrinsiceventindicators/assetpair")]
        Task EditAssetPairAsync(IntrinsicEventIndicatorsRowEdit row);
        [Delete("/api/lykkeintrinsiceventindicators/assetpair")]
        Task RemoveAssetPairAsync(string rowId);
        [Get("/api/lykkeintrinsiceventindicators/data")]
        Task<IntrinsicEventIndicatorsDto> GetDataAsync();
        [Get("/api/lykkeintrinsiceventindicators/runnersstates")]
        Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesAsync();
    }
}
