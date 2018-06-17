using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.IntrinsicEventIndicators.Settings.ServiceSettings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        [AzureTableCheck]
        public string DataConnString { get; set; }
    }
}
