using JetBrains.Annotations;
using Lykke.Service.IncreasticEventIndicators.Rabbit;

namespace Lykke.Service.IncreasticEventIndicators.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class IncreasticEventIndicatorsSettings
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
