using System.Threading.Tasks;
using Lykke.Common.ExchangeAdapter.Contracts;

namespace Lykke.Service.IncreasticEventIndicators.Core.Services.Exchanges
{
    public interface ITickPriceHandler
    {
        Task Handle(TickPrice tickPrice);
    }
}
