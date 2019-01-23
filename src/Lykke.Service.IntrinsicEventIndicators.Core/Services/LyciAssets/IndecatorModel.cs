using System.Collections.Generic;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Services.LyciAssets
{
    public class IndecatorList
    {
        public IndecatorList()
        {
            Indicators = new List<IndecatorModel>();
        }

        public List<IndecatorModel> Indicators { get; set; }
    }

    public class IndecatorModel
    {
        public IndecatorModel()
        {
        }

        public IndecatorModel(string assetPair, decimal delta, decimal overshot)
        {
            AssetPair = assetPair;
            Delta = delta;
            Overshot = overshot;
        }

        public string AssetPair { get; set; }
        public decimal Delta { get; set; }
        public decimal Overshot { get; set; }
    }
}
