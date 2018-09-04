using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public interface IEventHistoryRepository
    {
        Task Save(IEventHistory eventHistory);

        Task<IReadOnlyList<IEventHistory>> GetEventHistoryData(DateTime date, string exchange, string assetPair, decimal? delta);
    }
}
