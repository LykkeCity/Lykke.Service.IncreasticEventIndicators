using System;
using Common;
using Newtonsoft.Json;

namespace Lykke.Service.IncreasticEventIndicators.Core.Domain
{
    public class TickPrice
    {
        public TickPrice(DateTime time, decimal mid)
        {
            Time = time;
            Ask = mid;
            Bid = mid;
            Mid = mid;
        }

        [JsonConstructor]
        public TickPrice(Instrument instrument, DateTime time, decimal ask, decimal bid)
        {
            Time = time;
            Ask = ask;
            Bid = bid;
            Mid = (ask + bid) / 2m;

            Instrument = instrument;
        }

        public Instrument Instrument { get; }

        public DateTime Time { get; }

        public decimal Ask { get; }

        public decimal Bid { get; }

        [JsonIgnore]
        public decimal Mid { get; }

        [JsonIgnore]
        public decimal Spread => Ask - Bid;

        public override string ToString()
        {
            return new
            {
                AlphaEngine = new
                {
                    ContextMessage = "TickPrice",
                    Instrument = Instrument?.Name,
                    Instrument?.Exchange,
                    Time,
                    Ask,
                    Bid,
                    Mid
                }
            }.ToJson();
        }
    }
}
