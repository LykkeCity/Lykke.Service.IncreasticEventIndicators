using System.Threading.Tasks;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;

namespace Lykke.Service.IncreasticEventIndicators.Core.Services.Exchanges
{
    public interface ILykkeTickPriceHandler
    {
        Task Handle(ITickPrice tickPrice);
    }
}
