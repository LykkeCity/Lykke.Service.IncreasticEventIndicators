using Lykke.Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Services;

namespace Lykke.Service.IntrinsicEventIndicators.Services
{
    public class ExternalIntrinsicEventIndicatorsService : IntrinsicEventIndicatorsService, IExternalIntrinsicEventIndicatorsService
    {
        public ExternalIntrinsicEventIndicatorsService(IExternalIntrinsicEventIndicatorsRepository repo,
            IExternalMatrixHistoryRepository matrixHistoryRepo, IEventHistoryRepository eventHistoryRepo,
            IExternalTickPriceManager tickPriceManager, ILogFactory logFactory)
            : base(repo, matrixHistoryRepo, eventHistoryRepo, tickPriceManager, logFactory)
        {
        }
    }
}
