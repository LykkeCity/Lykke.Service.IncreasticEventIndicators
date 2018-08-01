using System;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain.Model
{
    public class IntrinsicEventIndicators
    {
        public IIntrinsicEventIndicatorsColumn[] Columns { get; set; }
        public IIntrinsicEventIndicatorsRow[] Rows { get; set; }
        public decimal[][] Data { get; set; }
        public TimeSpan?[][] TimesFromDc { get; set; }
    }
}
