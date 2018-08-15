using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.IntrinsicEventIndicators.Settings.ServiceSettings;

namespace Lykke.Service.IntrinsicEventIndicators.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public IntrinsicEventIndicatorsSettings IntrinsicEventIndicatorsService { get; set; }
    }
}
