using Lykke.Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IntrinsicEventIndicators.Settings.ServiceSettings;

namespace Lykke.Service.IntrinsicEventIndicators.Rabbit
{
    public class LykkeTickPriceSubscriber : TickPriceSubscriber
    {
        public LykkeTickPriceSubscriber(
            TickPriceExchangeSettings settings,
            ILykkeTickPriceHandler handler,
            ILogFactory logFactory)
            :base(settings, handler, logFactory)
        {
        }
    }
}
