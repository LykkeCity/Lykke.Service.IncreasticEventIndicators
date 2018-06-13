using System;
using Newtonsoft.Json;

namespace Lykke.Service.IncreasticEventIndicators.Core.Domain
{
    public class Instrument
    {
        public string Name { get; }
        public string Exchange { get; }
        public string Base { get; }
        public string Quote { get; }

        [JsonIgnore]
        public int? PriceDecimals { get; }

        [JsonIgnore]
        public int? VolumeDecimals { get; }

        [JsonIgnore]
        public decimal? MinDealVolume { get; }

        public Instrument(string exchange, string name, string @base, string quote,
            int? priceDecimals = null, int? volumeDecimals = null, decimal? minDealVolume = null)
        {
            Exchange = exchange;
            Name = name;
            Base = @base;
            Quote = quote;
            PriceDecimals = priceDecimals;
            VolumeDecimals = volumeDecimals;
            MinDealVolume = minDealVolume;
        }

        public override string ToString()
        {
            return $"{Name} on {Exchange}";
        }

        #region "Override equality comparison"

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Exchange.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((obj as Instrument)?.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase) ?? false)
                   && ((Instrument)obj).Exchange.Equals(Exchange, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool operator ==(Instrument left, Instrument right)
        {
            if ((object)left == (object)right) return true;
            if ((object)left == null || (object)right == null) return false;

            return left.Equals(right);
        }

        public static bool operator !=(Instrument left, Instrument right)
        {
            return !(left == right);
        }

        #endregion

        public bool IsReversedTo(Instrument another)
        {
            return Base != another.Base && Quote != another.Quote;
        }
    }
}
