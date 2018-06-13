using Common.Log;
using Lykke.Service.IncreasticEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IncreasticEventIndicators.Settings.ServiceSettings;

namespace Lykke.Service.IncreasticEventIndicators.Rabbit
{
    public class LykkeTickPriceSubscriber : TickPriceSubscriber
    {
        public LykkeTickPriceSubscriber(
            TickPriceExchangeSettings settings,
            ILykkeTickPriceHandler handler,
            ILog log)
            :base(settings, handler, log)
        {
        }
    }
}
