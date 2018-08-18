using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Client.Models;
using Refit;

namespace Lykke.Service.IntrinsicEventIndicators.Client.Api
{
    [PublicAPI]
    public interface IExternalIntrinsicEventIndicatorsApi
    {
        [Post("/api/externalintrinsiceventindicators/delta")]
        Task AddDeltaExternalAsync(IntrinsicEventIndicatorsColumnPost column);
        [Delete("/api/externalintrinsiceventindicators/delta")]
        Task RemoveDeltaExternalAsync(string columnId);
        [Post("/api/externalintrinsiceventindicators/assetpair")]
        Task AddAssetPairExternalAsync(IntrinsicEventIndicatorsRowPost row);
        [Put("/api/externalintrinsiceventindicators/assetpair")]
        Task EditAssetPairExternalAsync(IntrinsicEventIndicatorsRowEdit row);
        [Delete("/api/externalintrinsiceventindicators/assetpair")]
        Task RemoveAssetPairExternalAsync(string rowId);
        [Get("/api/externalintrinsiceventindicators/data")]
        Task<IntrinsicEventIndicatorsDto> GetDataExternalAsync();
        [Get("/api/externalintrinsiceventindicators/runnersstates")]
        Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesExternalAsync();
    }
}
