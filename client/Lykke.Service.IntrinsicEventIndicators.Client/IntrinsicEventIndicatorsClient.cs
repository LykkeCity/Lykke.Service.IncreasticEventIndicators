using Lykke.HttpClientGenerator;
using Lykke.Service.IntrinsicEventIndicators.Client.Api;

namespace Lykke.Service.IntrinsicEventIndicators.Client
{
    /// <summary>
    /// IntrinsicEventIndicators API aggregating interface.
    /// </summary>
    public class IntrinsicEventIndicatorsClient : IIntrinsicEventIndicatorsClient
    {
        /// <summary>Api for LykkeIntrinsicEventIndicators</summary>
        public ILykkeIntrinsicEventIndicatorsApi LykkeIntrinsicEventIndicatorsApi { get; private set; }

        /// <summary>Api for ExternalIntrinsicEventIndicators</summary>
        public IExternalIntrinsicEventIndicatorsApi ExternalIntrinsicEventIndicatorsApi { get; private set; }

        /// <summary>C-tor</summary>
        public IntrinsicEventIndicatorsClient(IHttpClientGenerator httpClientGenerator)
        {
            LykkeIntrinsicEventIndicatorsApi = httpClientGenerator.Generate<ILykkeIntrinsicEventIndicatorsApi>();
            ExternalIntrinsicEventIndicatorsApi = httpClientGenerator.Generate<IExternalIntrinsicEventIndicatorsApi>();
        }
    }
}
