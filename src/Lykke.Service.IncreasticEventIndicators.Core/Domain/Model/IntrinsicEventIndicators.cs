namespace Lykke.Service.IncreasticEventIndicators.Core.Domain.Model
{
    public class IntrinsicEventIndicators
    {
        public IIntrinsicEventIndicatorsColumn[] Columns { get; set; }
        public string[][] Data { get; set; }
    }
}
