using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public interface IMatrixHistoryRepository
    {
        Task Save(IMatrixHistory matrixHistory);

        Task<IList<DateTime>> GetMatrixHistoryStamps(DateTime date);

        Task<string> GetMatrixHistoryData(DateTime date);
    }
}
