using Common.Log;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;
using Lykke.Service.IncreasticEventIndicators.Core.Services.Exchanges;

namespace Lykke.Service.IncreasticEventIndicators.Services.Exchanges
{
    public class LykkeTickPriceManager : TickPriceManager, ILykkeTickPriceManager, ILykkeTickPriceHandler
    {
        public LykkeTickPriceManager(ILog log, ILykkeRunnerStateRepository runnerStateRepository)
            : base(log, runnerStateRepository)
        {
        }

        protected override string ParseRunnersStatesKeyFromRunnersKey(string runnersKey)
        {
            var exchangeAssetPairKey = ParseExchangeAssetPairFromRunnersKey(runnersKey);
            return ParseAssetPairFromExchangeAssetPairKey(exchangeAssetPairKey);
        }
    }
}
