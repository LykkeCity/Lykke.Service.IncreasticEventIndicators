using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;

namespace Lykke.Service.IncreasticEventIndicators.Core.Domain
{
    public interface IIntrinsicEventIndicatorsRepository
    {
        Task<IEnumerable<IIntrinsicEventIndicatorsColumn>> GetColumnsAsync();
        Task AddColumnAsync(IIntrinsicEventIndicatorsColumn column);
        Task RemoveColumnAsync(string columnId);
    }
}
