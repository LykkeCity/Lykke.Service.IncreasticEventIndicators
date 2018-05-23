using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.IncreasticEventIndicators.Settings.ServiceSettings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }
    }
}
