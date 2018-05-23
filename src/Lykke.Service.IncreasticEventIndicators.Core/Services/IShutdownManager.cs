using System.Threading.Tasks;

namespace Lykke.Service.IncreasticEventIndicators.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}
