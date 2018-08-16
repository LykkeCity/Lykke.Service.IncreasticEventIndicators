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
        [Post("/api/intrinsiceventindicatorsdeltaexternal")]
        Task AddDeltaExternalAsync(IntrinsicEventIndicatorsColumnPost column);
        [Delete("/api/intrinsiceventindicatorsdeltaexternal")]
        Task RemoveDeltaExternalAsync(string columnId);
        [Post("/api/intrinsiceventindicatorsassetpairexternal")]
        Task AddAssetPairExternalAsync(IntrinsicEventIndicatorsRowPost row);
        [Put("/api/intrinsiceventindicatorsassetpairexternal")]
        Task EditAssetPairExternalAsync(IntrinsicEventIndicatorsRowEdit row);
        [Delete("/api/intrinsiceventindicatorsassetpairexternal")]
        Task RemoveAssetPairExternalAsync(string rowId);
        [Get("/api/intrinsiceventindicatorsdataexternal")]
        Task<IntrinsicEventIndicatorsDto> GetDataExternalAsync();
        [Get("/api/intrinsiceventindicatorsrunnersstatesexternal")]
        Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesExternalAsync();
    }
}
