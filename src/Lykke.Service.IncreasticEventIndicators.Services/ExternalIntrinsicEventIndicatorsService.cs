using Common.Log;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;
using Lykke.Service.IncreasticEventIndicators.Core.Services;

namespace Lykke.Service.IncreasticEventIndicators.Services
{
    public class ExternalIntrinsicEventIndicatorsService : IntrinsicEventIndicatorsService, IExternalIntrinsicEventIndicatorsService
    {
        public ExternalIntrinsicEventIndicatorsService(IExternalIntrinsicEventIndicatorsRepository repo,
            IExternalTickPriceManager tickPriceManager, ILog log)
            : base(repo, tickPriceManager, log)
        {
        }
    }
}
