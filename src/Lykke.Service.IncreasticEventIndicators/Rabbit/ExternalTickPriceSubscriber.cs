using Common.Log;
using Lykke.Service.IncreasticEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IncreasticEventIndicators.Settings.ServiceSettings;

namespace Lykke.Service.IncreasticEventIndicators.Rabbit
{
    public class ExternalTickPriceSubscriber : TickPriceSubscriber
    {
        public ExternalTickPriceSubscriber(
            TickPriceExchangeSettings settings,
            IExternalTickPriceHandler handler,
            ILog log)
            :base(settings, handler, log)
        {
        }
    }
}
