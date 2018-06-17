using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Settings.ServiceSettings;
using Lykke.Service.IntrinsicEventIndicators.Settings.SlackNotifications;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.IntrinsicEventIndicators.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings
    {
        public IntrinsicEventIndicatorsSettings IntrinsicEventIndicatorsService { get; set; }

        public SlackNotificationsSettings SlackNotifications { get; set; }

        [Optional]
        public MonitoringServiceClientSettings MonitoringServiceClient { get; set; }
    }
}
