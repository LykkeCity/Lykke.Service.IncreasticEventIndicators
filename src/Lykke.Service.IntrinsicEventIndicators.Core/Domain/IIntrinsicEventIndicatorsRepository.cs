using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain.Model;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public interface IIntrinsicEventIndicatorsRepository
    {
        Task<IEnumerable<IIntrinsicEventIndicatorsColumn>> GetColumnsAsync();
        Task<IEnumerable<IIntrinsicEventIndicatorsRow>> GetRowsAsync();
        Task AddColumnAsync(IIntrinsicEventIndicatorsColumn column);
        Task RemoveColumnAsync(string columnId);
        Task AddAssetPairAsync(IIntrinsicEventIndicatorsRow row);
        Task RemoveAssetPairAsync(string rowId);
    }
}
