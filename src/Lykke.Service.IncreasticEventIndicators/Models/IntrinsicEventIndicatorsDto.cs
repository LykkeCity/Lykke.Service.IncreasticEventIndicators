namespace Lykke.Service.IncreasticEventIndicators.Models
{
    public class IntrinsicEventIndicatorsDto
    {
        public IntrinsicEventIndicatorsColumnDto[] Columns { get; set; }
        public IntrinsicEventIndicatorsRowDto[] Rows { get; set; }
        public decimal[][] Data { get; set; }
    }
}
