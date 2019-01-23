using Lykke.Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.LyciAssets;
using Lykke.Service.IntrinsicEventIndicators.Settings.ServiceSettings;

namespace Lykke.Service.IntrinsicEventIndicators.Rabbit
{
    public class LykkeTickPriceSubscriber : TickPriceSubscriber
    {
        public LykkeTickPriceSubscriber(
            TickPriceExchangeSettings settings,
            ILykkeTickPriceHandler handler,
            IPriceManager priceManager,
            ILogFactory logFactory)
            :base(settings, handler, priceManager, logFactory)
        {
        }
    }
}
