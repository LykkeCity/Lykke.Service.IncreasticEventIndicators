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
        [Post("/api/intrinsiceventindicators/delta")]
        Task AddDeltaAsync(IntrinsicEventIndicatorsColumnPost column);
        [Delete("/api/intrinsiceventindicators/delta")]
        Task RemoveDeltaAsync(string columnId);
        [Post("/api/intrinsiceventindicators/assetpair")]
        Task AddAssetPairAsync(IntrinsicEventIndicatorsRowPost row);
        [Put("/api/intrinsiceventindicators/assetpair")]
        Task EditAssetPairAsync(IntrinsicEventIndicatorsRowEdit row);
        [Delete("/api/intrinsiceventindicators/assetpair")]
        Task RemoveAssetPairAsync(string rowId);
        [Get("/api/intrinsiceventindicators/data")]
        Task<IntrinsicEventIndicatorsDto> GetDataAsync();
        [Get("/api/intrinsiceventindicators/runnersstates")]
        Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesAsync();
    }
}
