using Lykke.Service.IntrinsicEventIndicators.Core.Domain;

namespace Lykke.Service.IntrinsicEventIndicators.Models
{
    public class RunnerStateDto
    {
        public int Event { get; set; }
        public decimal Extreme { get; set; }
        public decimal ExpectedDcLevel { get; set; }
        public decimal ExpectedOsLevel { get; set; }
        public decimal Reference { get; set; }
        public ExpectedDirectionalChange ExpectedDirectionalChange { get; set; }
        public decimal DirectionalChangePrice { get; set; }
        public decimal Delta { get; set; }
        public string AssetPair { get; set; }
        public string Exchange { get; set; }
    }
}
