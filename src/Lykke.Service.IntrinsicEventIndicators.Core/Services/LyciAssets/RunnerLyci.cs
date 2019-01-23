using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Services.LyciAssets
{
    public class RunnerLyci : Runner
    {
        public RunnerLyci(decimal delta, string assetPair, ILog log) 
            : base(delta, assetPair, string.Empty, new MockEventHistoryRepository(), log)
        {
        }

        public RunnerLyci(RunnerState state, ILog log) 
            : base(state, new MockEventHistoryRepository(), log)
        {
        }


        public class MockEventHistoryRepository : IEventHistoryRepository
        {
            public Task Save(IEventHistory eventHistory)
            {
                return Task.CompletedTask;
            }

            public Task<IReadOnlyList<IEventHistory>> GetEventHistoryData(DateTime date, string exchange, string assetPair, decimal? delta)
            {
                throw new NotImplementedException();
            }
        }
    }
}
