using Lykke.Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;

namespace Lykke.Service.IntrinsicEventIndicators.Services.Exchanges
{
    public class LykkeTickPriceManager : TickPriceManager, ILykkeTickPriceManager, ILykkeTickPriceHandler
    {
        public LykkeTickPriceManager(ILogFactory logFactory, ILykkeRunnerStateRepository runnerStateRepository,
            ILykkeMatrixHistoryRepository matrixHistoryRepository,
            ILykkeIntrinsicEventIndicatorsRepository repo)
            : base(logFactory, runnerStateRepository, matrixHistoryRepository, repo)
        {
        }

        protected override string ParseRunnersStatesKeyFromRunnersKey(string runnersKey)
        {
            var exchangeAssetPairKey = ParseExchangeAssetPairFromRunnersKey(runnersKey);
            return ParseAssetPairFromExchangeAssetPairKey(exchangeAssetPairKey);
        }
    }
}
