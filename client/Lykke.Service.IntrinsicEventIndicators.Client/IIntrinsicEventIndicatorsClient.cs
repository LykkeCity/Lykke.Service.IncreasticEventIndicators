using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Client.Api;

namespace Lykke.Service.IntrinsicEventIndicators.Client
{
    /// <summary>
    /// IntrinsicEventIndicators client interface.
    /// </summary>
    [PublicAPI]
    public interface IIntrinsicEventIndicatorsClient
    {
        /// <summary>Api for LykkeIntrinsicEventIndicators</summary>
        ILykkeIntrinsicEventIndicatorsApi LykkeIntrinsicEventIndicatorsApi { get; }

        IExternalIntrinsicEventIndicatorsApi ExternalIntrinsicEventIndicatorsApi { get; }
    }
}
