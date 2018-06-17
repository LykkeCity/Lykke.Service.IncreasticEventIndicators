using Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Services;

namespace Lykke.Service.IntrinsicEventIndicators.Services
{
    public class LykkeIntrinsicEventIndicatorsService : IntrinsicEventIndicatorsService, ILykkeIntrinsicEventIndicatorsService
    {
        public LykkeIntrinsicEventIndicatorsService(ILykkeIntrinsicEventIndicatorsRepository repo,
            ILykkeTickPriceManager tickPriceManager, ILog log)
            : base(repo, tickPriceManager, log)
        {
        }
    }
}
