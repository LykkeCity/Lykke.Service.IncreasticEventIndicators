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
        [Post("/api/intrinsiceventindicatorsdelta")]
        Task AddDeltaAsync(IntrinsicEventIndicatorsColumnPost column);
        [Delete("/api/intrinsiceventindicatorsdelta")]
        Task RemoveDeltaAsync(string columnId);
        [Post("/api/intrinsiceventindicatorsassetpair")]
        Task AddAssetPairAsync(IntrinsicEventIndicatorsRowPost row);
        [Put("/api/intrinsiceventindicatorsassetpair")]
        Task EditAssetPairAsync(IntrinsicEventIndicatorsRowEdit row);
        [Delete("/api/intrinsiceventindicatorsassetpair")]
        Task RemoveAssetPairAsync(string rowId);
        [Get("/api/intrinsiceventindicatorsdata")]
        Task<IntrinsicEventIndicatorsDto> GetDataAsync();
        [Get("/api/intrinsiceventindicatorsrunnersstates")]
        Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesAsync();
    }
}
