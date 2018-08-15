using Lykke.Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IntrinsicEventIndicators.Settings.ServiceSettings;

namespace Lykke.Service.IntrinsicEventIndicators.Rabbit
{
    public class ExternalTickPriceSubscriber : TickPriceSubscriber
    {
        public ExternalTickPriceSubscriber(
            TickPriceExchangeSettings settings,
            IExternalTickPriceHandler handler,
            ILogFactory logFactory)
            :base(settings, handler, logFactory)
        {
        }
    }
}
