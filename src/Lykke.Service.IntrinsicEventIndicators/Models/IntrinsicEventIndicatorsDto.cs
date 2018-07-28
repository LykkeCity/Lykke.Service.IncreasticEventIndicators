using System;

namespace Lykke.Service.IntrinsicEventIndicators.Models
{
    public class IntrinsicEventIndicatorsDto
    {
        public IntrinsicEventIndicatorsColumnDto[] Columns { get; set; }
        public IntrinsicEventIndicatorsRowDto[] Rows { get; set; }
        public decimal[][] Data { get; set; }
        public TimeSpan?[][] TimesFromDc { get; set; }
    }
}
