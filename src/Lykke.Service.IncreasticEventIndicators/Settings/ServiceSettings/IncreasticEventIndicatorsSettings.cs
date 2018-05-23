using JetBrains.Annotations;

namespace Lykke.Service.IncreasticEventIndicators.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class IncreasticEventIndicatorsSettings
    {
        public DbSettings Db { get; set; }
    }
}
