using Lykke.Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Services;

namespace Lykke.Service.IntrinsicEventIndicators.Services
{
    public class LykkeIntrinsicEventIndicatorsService : IntrinsicEventIndicatorsService, ILykkeIntrinsicEventIndicatorsService
    {
        public LykkeIntrinsicEventIndicatorsService(ILykkeIntrinsicEventIndicatorsRepository repo,
            ILykkeMatrixHistoryRepository matrixHistoryRepo,
            ILykkeTickPriceManager tickPriceManager, ILogFactory logFactory)
            : base(repo, matrixHistoryRepo, tickPriceManager, logFactory)
        {
        }
    }
}
