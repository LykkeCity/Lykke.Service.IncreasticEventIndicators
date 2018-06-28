using System.Threading.Tasks;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Services
{
    public interface IStartupManager
    {
        Task StartAsync();
    }
}