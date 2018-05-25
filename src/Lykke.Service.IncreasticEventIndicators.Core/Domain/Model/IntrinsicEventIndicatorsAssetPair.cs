namespace Lykke.Service.IncreasticEventIndicators.Core.Domain.Model
{
    public interface IIntrinsicEventIndicatorsAssetPair
    {
        string RowId { get; }
        string AssetPair { get; }
    }

    public class IntrinsicEventIndicatorsAssetPair : IIntrinsicEventIndicatorsAssetPair
    {
        public string RowId { get; set; }
        public string AssetPair { get; set; }
    }
}
