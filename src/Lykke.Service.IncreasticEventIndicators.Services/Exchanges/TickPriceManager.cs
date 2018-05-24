using System.Threading.Tasks;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;
using Lykke.Service.IncreasticEventIndicators.Core.Services.Exchanges;

namespace Lykke.Service.IncreasticEventIndicators.Services.Exchanges
{
    public class TickPriceManager : ITickPriceManager, ILykkeTickPriceHandler
    {
        public ITickPrice TickPrice { get; private set; }

        public Task<ITickPrice> GetCurrentTickPrice(string assetPair)
        {
            //if (TickPrice?.AssetPair?.Name != assetPair)
            //{
            //    return Task.FromResult((ITickPrice)null);
            //}

            return Task.FromResult(TickPrice);
        }

        public Task Handle(ITickPrice tickPrice)
        {
            TickPrice = tickPrice;
            return Task.CompletedTask;
        }
    }
}
