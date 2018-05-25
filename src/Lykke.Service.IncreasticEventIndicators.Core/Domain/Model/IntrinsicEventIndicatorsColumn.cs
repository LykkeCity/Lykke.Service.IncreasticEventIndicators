namespace Lykke.Service.IncreasticEventIndicators.Core.Domain.Model
{
    public interface IIntrinsicEventIndicatorsColumn
    {
        string ColumnId { get; }
        decimal Delta { get; }
    }

    public class IntrinsicEventIndicatorsColumn : IIntrinsicEventIndicatorsColumn
    {
        public string ColumnId { get; set; }
        public decimal Delta { get; set; }
    }
}
