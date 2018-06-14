using JetBrains.Annotations;
using Lykke.Service.IncreasticEventIndicators.Settings.ServiceSettings;
using Lykke.Service.IncreasticEventIndicators.Settings.SlackNotifications;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.IncreasticEventIndicators.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings
    {
        public IncreasticEventIndicatorsSettings IntrinsicEventIndicatorsService { get; set; }

        public SlackNotificationsSettings SlackNotifications { get; set; }

        [Optional]
        public MonitoringServiceClientSettings MonitoringServiceClient { get; set; }
    }
}
