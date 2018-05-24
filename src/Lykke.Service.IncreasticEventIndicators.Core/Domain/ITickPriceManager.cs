using System.Threading.Tasks;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;

namespace Lykke.Service.IncreasticEventIndicators.Core.Domain
{
    public interface ITickPriceManager
    {
        Task<ITickPrice> GetCurrentTickPrice(string assetPair);
    }
}
