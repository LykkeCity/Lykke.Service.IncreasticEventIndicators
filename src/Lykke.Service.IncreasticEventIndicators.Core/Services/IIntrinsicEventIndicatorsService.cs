using System.Threading.Tasks;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;

namespace Lykke.Service.IncreasticEventIndicators.Core.Services
{
    public interface IIntrinsicEventIndicatorsService
    {
        Task AddColumn(IIntrinsicEventIndicatorsColumn column);
        Task RemoveColumn(string columnId);

        Task<IntrinsicEventIndicators> GetData();
    }
}
