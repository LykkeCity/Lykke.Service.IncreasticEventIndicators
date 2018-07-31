using Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;

namespace Lykke.Service.IntrinsicEventIndicators.Services.Exchanges
{
    public class ExternalTickPriceManager : TickPriceManager, IExternalTickPriceManager, IExternalTickPriceHandler
    {
        public ExternalTickPriceManager(ILog log, IExternalRunnerStateRepository runnerStateRepository,
            IIntrinsicEventIndicatorsRepository repo)
            : base(log, runnerStateRepository, repo)
        {
        }

        protected override string ParseRunnersStatesKeyFromRunnersKey(string runnersKey)
        {
            return ParseExchangeAssetPairFromRunnersKey(runnersKey);
        }
    }
}
