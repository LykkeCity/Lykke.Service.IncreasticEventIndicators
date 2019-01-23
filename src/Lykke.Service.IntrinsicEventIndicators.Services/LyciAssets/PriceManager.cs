using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Common.ExchangeAdapter.Contracts;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.LyciAssets;

namespace Lykke.Service.IntrinsicEventIndicators.Services.LyciAssets
{
    public class PriceManager : IPriceManager
    {
        private readonly object _gate = new object();
        private readonly Dictionary<string, Dictionary<string, decimal>> _prices = new Dictionary<string, Dictionary<string, decimal>>();

        public Task Handle(TickPrice tickPrice)
        {
            lock (_gate)
            {
                if (!tickPrice.Asset.EndsWith("USD"))
                    return Task.CompletedTask;

                if (!_prices.ContainsKey(tickPrice.Asset))
                {
                    _prices[tickPrice.Asset] = new Dictionary<string, decimal>();
                }

                var price = _prices[tickPrice.Asset];

                price[tickPrice.Source] = (tickPrice.Ask + tickPrice.Bid) / 2;

                return Task.CompletedTask;
            }
        }

        public PriceValue GetPrice(string asset)
        {
            lock (_gate)
            {
                if (!_prices.ContainsKey(asset))
                    return null;

                var prices = _prices[asset];

                if (!prices.Any())
                    return null;

                var values = (prices.Count >= 4)
                    ? prices.Values.OrderBy(e => e).Skip(1).Take(prices.Count - 2)
                    : prices.Values;

                return new PriceValue(asset, values.Average());
            }
        }

        public List<PriceValue> GetPrices()
        {
            lock (_gate)
            {
                var data = _prices.Keys.Select(GetPrice).ToList();
                return data;
            }
        }
    }
}
