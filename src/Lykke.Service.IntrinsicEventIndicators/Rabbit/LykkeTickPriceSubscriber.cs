using Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IntrinsicEventIndicators.Settings.ServiceSettings;

namespace Lykke.Service.IntrinsicEventIndicators.Rabbit
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
