using Lykke.Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.LyciAssets;
using Lykke.Service.IntrinsicEventIndicators.Settings.ServiceSettings;

namespace Lykke.Service.IntrinsicEventIndicators.Rabbit
{
    public class ExternalTickPriceSubscriber : TickPriceSubscriber
    {
        public ExternalTickPriceSubscriber(
            TickPriceExchangeSettings settings,
            IExternalTickPriceHandler handler,
            IPriceManager priceManager,
            ILogFactory logFactory)
            :base(settings, handler, priceManager, logFactory)
        {
        }
    }
}
