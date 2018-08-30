using System;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain.Model
{
    public class IntrinsicEventIndicators
    {
        public IntrinsicEventIndicatorsColumn[] Columns { get; set; }
        public IntrinsicEventIndicatorsRow[] Rows { get; set; }
        public decimal[][] Data { get; set; }
        public TimeSpan?[][] TimesFromDc { get; set; }
    }
}
