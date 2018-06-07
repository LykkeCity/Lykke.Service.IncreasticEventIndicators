using JetBrains.Annotations;
using Lykke.Service.IncreasticEventIndicators.Rabbit;

namespace Lykke.Service.IncreasticEventIndicators.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class IncreasticEventIndicatorsSettings
    {
        public DbSettings Db { get; set; }

        public ExchangeSettings TickPrice { get; set; }
    }
}
