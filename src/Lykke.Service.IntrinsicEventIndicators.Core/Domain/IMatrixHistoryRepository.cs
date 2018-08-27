using System.Threading.Tasks;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public interface IMatrixHistoryRepository
    {
        Task Save(IMatrixHistory matrixHistory);
    }
}
