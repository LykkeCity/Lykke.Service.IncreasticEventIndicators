using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain.Model;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Services
{
    public interface IIntrinsicEventIndicatorsService
    {
        Task AddColumn(IIntrinsicEventIndicatorsColumn column);
        Task RemoveColumn(string columnId);

        Task AddAssetPair(IIntrinsicEventIndicatorsRow row);
        Task EditAssetPair(IIntrinsicEventIndicatorsRow row);
        Task RemoveAssetPair(string rowId);

        Task<Domain.Model.IntrinsicEventIndicators> GetData();

        Task<IDictionary<string, IList<IRunnerState>>> GetRunnersStates();

        Task<IList<DateTime>> GetMatrixHistoryStamps(DateTime date);
        Task<Domain.Model.IntrinsicEventIndicators> GetMatrixHistoryData(DateTime date);
    }
}
