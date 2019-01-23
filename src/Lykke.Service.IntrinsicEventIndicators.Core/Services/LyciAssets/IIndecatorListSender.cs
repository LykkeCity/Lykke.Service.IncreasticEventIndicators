using System.Threading.Tasks;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Services.LyciAssets
{
    public interface IIndecatorListSender
    {
        Task SendAsync(IndecatorList message);
    }
}
