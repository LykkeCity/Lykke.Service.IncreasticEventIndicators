namespace Lykke.Service.IncreasticEventIndicators.Core.Domain.Model
{
    public interface IIntrinsicEventIndicatorsColumn
    {
        string ColumnId { get; }
        decimal Value { get; }
    }

    public class IntrinsicEventIndicatorsColumn : IIntrinsicEventIndicatorsColumn
    {
        public string ColumnId { get; set; }
        public decimal Value { get; set; }
    }
}
