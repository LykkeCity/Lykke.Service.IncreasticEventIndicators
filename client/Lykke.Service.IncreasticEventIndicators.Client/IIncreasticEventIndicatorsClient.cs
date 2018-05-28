
using System;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.IncreasticEventIndicators.Client.AutorestClient.Models;

namespace Lykke.Service.IncreasticEventIndicators.Client
{
    public interface IIncreasticEventIndicatorsClient : IDisposable
    {
        /// <summary>
        /// Checks if service is alive
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>IsAlive response</returns>
        /// <exception cref="Exceptions.ApiException">Thrown on getting error response.</exception>
        /// <exception cref="Microsoft.Rest.HttpOperationException">Thrown on getting incorrect http response.</exception>
        /// <exception cref="OperationCanceledException">Thrown on canceled token</exception>
        Task<IsAliveResponse> IsAliveAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds delta.
        /// </summary>
        /// <param name="column">Delta to add.</param>
        /// <param name="cancellationToken"></param>
        Task AddDeltaAsync(IntrinsicEventIndicatorsColumnPost column, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes delta.
        /// </summary>
        /// <param name="columnId">delta</param>
        /// <param name="cancellationToken"></param>
        Task RemoveDeltaAsync(string columnId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds asset pair.
        /// </summary>
        /// <param name="row">Asset pair to add.</param>
        /// <param name="cancellationToken"></param>
        Task AddAssetPairAsync(IntrinsicEventIndicatorsAssetPairPost row, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes asset pair.
        /// </summary>
        /// <param name="rowId">asset pair</param>
        /// <param name="cancellationToken"></param>
        Task RemoveAssetPairAsync(string rowId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Data</returns>
        Task<IntrinsicEventIndicatorsDto> GetDataAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
