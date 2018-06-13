using Common.Log;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;
using Lykke.Service.IncreasticEventIndicators.Core.Services.Exchanges;

namespace Lykke.Service.IncreasticEventIndicators.Services.Exchanges
{
    public class ExternalTickPriceManager : TickPriceManager, IExternalTickPriceManager, IExternalTickPriceHandler
    {
        public ExternalTickPriceManager(ILog log, IExternalRunnerStateRepository runnerStateRepository)
            : base(log, runnerStateRepository)
        {
        }

        protected override string ParseRunnersStatesKeyFromRunnersKey(string runnersKey)
        {
            return ParseExchangeAssetPairFromRunnersKey(runnersKey);
        }
    }
}
