using Lykke.Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;

namespace Lykke.Service.IntrinsicEventIndicators.Services.Exchanges
{
    public class ExternalTickPriceManager : TickPriceManager, IExternalTickPriceManager, IExternalTickPriceHandler
    {
        public ExternalTickPriceManager(ILogFactory logFactory, IExternalRunnerStateRepository runnerStateRepository,
            IExternalMatrixHistoryRepository matrixHistoryRepository,
            IExternalIntrinsicEventIndicatorsRepository repo)
            : base(logFactory, runnerStateRepository, matrixHistoryRepository, repo)
        {
        }

        protected override string ParseRunnersStatesKeyFromRunnersKey(string runnersKey)
        {
            return ParseExchangeAssetPairFromRunnersKey(runnersKey);
        }
    }
}
