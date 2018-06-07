namespace Lykke.Service.IncreasticEventIndicators.Core.Domain.Model
{
    public class IntrinsicEventIndicators
    {
        public IIntrinsicEventIndicatorsColumn[] Columns { get; set; }
        public IIntrinsicEventIndicatorsAssetPair[] AssetPairs { get; set; }
        public decimal[][] Data { get; set; }
    }
}
