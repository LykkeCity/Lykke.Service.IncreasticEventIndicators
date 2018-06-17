namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain.Model
{
    public interface IIntrinsicEventIndicatorsRow
    {
        string RowId { get; }
        string AssetPair { get; }
        string Exchange { get; }
    }

    public class IntrinsicEventIndicatorsRow : IIntrinsicEventIndicatorsRow
    {
        public string RowId { get; set; }
        public string AssetPair { get; set; }
        public string Exchange { get; set; }
    }
}
