using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.IncreasticEventIndicators.Core.Domain;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;
using Lykke.Service.IncreasticEventIndicators.Core.Services;
using Lykke.Service.IncreasticEventIndicators.Core.Services.Exchanges;

namespace Lykke.Service.IncreasticEventIndicators.Services
{
    public class IntrinsicEventIndicatorsService : IIntrinsicEventIndicatorsService
    {
        private readonly IIntrinsicEventIndicatorsRepository _repo;
        private readonly ILog _log;
        private readonly ILykkeTickPriceHandler _lykkeTickPriceHandler;

        public IntrinsicEventIndicatorsService(IIntrinsicEventIndicatorsRepository repo,
            ILykkeTickPriceHandler lykkeTickPriceHandler, ILog log)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _log = log.CreateComponentScope(nameof(IntrinsicEventIndicatorsService));
            _lykkeTickPriceHandler = lykkeTickPriceHandler;
        }

        public async Task AddColumn(IIntrinsicEventIndicatorsColumn column)
        {
            await _repo.AddColumnAsync(column);
        }

        public async Task RemoveColumn(string columnId)
        {
            await _repo.RemoveColumnAsync(columnId);
        }

        public async Task<IntrinsicEventIndicators> GetData()
        {
            var columns = (await _repo.GetColumnsAsync()).OrderBy(x => x.Value).ToList();

            var data = Enumerable.Range(0, 1).Select(x => columns.Select(c => 0M.ToString(CultureInfo.InvariantCulture)).ToArray()).ToArray();

            return await Task.FromResult(new IntrinsicEventIndicators
            {
                Columns = columns.ToArray(),
                Data = data
            });
        }
    }
}
