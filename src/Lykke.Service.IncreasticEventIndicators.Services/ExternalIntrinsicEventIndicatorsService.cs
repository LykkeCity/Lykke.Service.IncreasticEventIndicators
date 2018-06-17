using Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Services;

namespace Lykke.Service.IntrinsicEventIndicators.Services
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
