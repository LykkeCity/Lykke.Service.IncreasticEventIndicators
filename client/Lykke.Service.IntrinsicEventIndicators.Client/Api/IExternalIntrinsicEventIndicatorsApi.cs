using System;
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
        [Post("/api/v1/externalintrinsiceventindicators/intrinsiceventindicatorsdeltaexternal")]
        Task AddDeltaExternalAsync(IntrinsicEventIndicatorsColumnPost column);
        [Delete("/api/v1/externalintrinsiceventindicators/intrinsiceventindicatorsdeltaexternal")]
        Task RemoveDeltaExternalAsync(string columnId);
        [Post("/api/v1/externalintrinsiceventindicators/intrinsiceventindicatorsassetpairexternal")]
        Task AddAssetPairExternalAsync(IntrinsicEventIndicatorsRowPost row);
        [Put("/api/v1/externalintrinsiceventindicators/intrinsiceventindicatorsassetpairexternal")]
        Task EditAssetPairExternalAsync(IntrinsicEventIndicatorsRowEdit row);
        [Delete("/api/v1/externalintrinsiceventindicators/intrinsiceventindicatorsassetpairexternal")]
        Task RemoveAssetPairExternalAsync(string rowId);
        [Get("/api/v1/externalintrinsiceventindicators/intrinsiceventindicatorsdataexternal")]
        Task<IntrinsicEventIndicatorsDto> GetDataExternalAsync();
        [Get("/api/v1/externalintrinsiceventindicators/intrinsiceventindicatorsrunnersstatesexternal")]
        Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesExternalAsync();
        [Get("/api/v1/externalintrinsiceventindicators/matrixhistorystampsexternal")]
        Task<IList<DateTime>> GetMatrixHistoryStampsExternalAsync(DateTime date);
        [Get("/api/v1/externalintrinsiceventindicators/matrixhistorydataexternal")]
        Task<IntrinsicEventIndicatorsDto> GetMatrixHistoryDataExternalAsync(DateTime date);
        [Get("/api/v1/externalintrinsiceventindicators/eventhistorydataexternal")]
        Task<IReadOnlyList<EventHistoryDto>> GetEventHistoryDataExternalAsync(DateTime date, string exchange,
            string assetPair, decimal? delta);
    }
}
