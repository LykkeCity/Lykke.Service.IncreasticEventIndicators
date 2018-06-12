using Common.Log;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;
using Lykke.Service.IncreasticEventIndicators.Core.Services;

namespace Lykke.Service.IncreasticEventIndicators.Services
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
