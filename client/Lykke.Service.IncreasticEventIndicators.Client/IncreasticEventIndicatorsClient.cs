using System;
using Common.Log;

namespace Lykke.Service.IncreasticEventIndicators.Client
{
    public class IncreasticEventIndicatorsClient : IIncreasticEventIndicatorsClient, IDisposable
    {
        private readonly ILog _log;

        public IncreasticEventIndicatorsClient(string serviceUrl, ILog log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}
