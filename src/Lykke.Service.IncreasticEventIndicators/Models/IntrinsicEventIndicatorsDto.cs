namespace Lykke.Service.IncreasticEventIndicators.Models
{
    public class IntrinsicEventIndicatorsDto
    {
        public IntrinsicEventIndicatorsColumnDto[] Columns { get; set; }
        public IntrinsicEventIndicatorsAssetPairDto[] AssetPairs { get; set; }
        public string[][] Data { get; set; }
    }
}
