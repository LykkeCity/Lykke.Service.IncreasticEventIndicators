using System.Threading.Tasks;
using Lykke.Common.ExchangeAdapter.Contracts;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges
{
    public interface ITickPriceHandler
    {
        Task Handle(TickPrice tickPrice);
    }
}
