using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Rabbit;

namespace Lykke.Service.IntrinsicEventIndicators.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class IntrinsicEventIndicatorsSettings
    {
        public DbSettings Db { get; set; }

        public TickPriceExchangeSettings LykkeTickPriceExchange { get; set; }

        public TickPriceExchangeSettings[] ExternalTickPriceExchanges { get; set; }
    }

    public class TickPriceExchangeSettings
    {
        public ExchangeSettings Exchange { get; set; }
    }
}
