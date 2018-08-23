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
        [Post("/api/v1/lykkeintrinsiceventindicators/intrinsiceventindicatorsdelta")]
        Task AddDeltaAsync(IntrinsicEventIndicatorsColumnPost column);
        [Delete("/api/v1/lykkeintrinsiceventindicators/intrinsiceventindicatorsdelta")]
        Task RemoveDeltaAsync(string columnId);
        [Post("/api/v1/lykkeintrinsiceventindicators/intrinsiceventindicatorsassetpair")]
        Task AddAssetPairAsync(IntrinsicEventIndicatorsRowPost row);
        [Put("/api/v1/lykkeintrinsiceventindicators/intrinsiceventindicatorsassetpair")]
        Task EditAssetPairAsync(IntrinsicEventIndicatorsRowEdit row);
        [Delete("/api/v1/lykkeintrinsiceventindicators/intrinsiceventindicatorsassetpair")]
        Task RemoveAssetPairAsync(string rowId);
        [Get("/api/v1/lykkeintrinsiceventindicators/intrinsiceventindicatorsdata")]
        Task<IntrinsicEventIndicatorsDto> GetDataAsync();
        [Get("/api/v1/lykkeintrinsiceventindicators/intrinsiceventindicatorsrunnersstates")]
        Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesAsync();
    }
}
