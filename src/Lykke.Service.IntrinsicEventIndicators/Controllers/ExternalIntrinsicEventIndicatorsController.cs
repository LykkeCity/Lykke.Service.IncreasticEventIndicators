using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.IntrinsicEventIndicators.Client.Api;
using Lykke.Service.IntrinsicEventIndicators.Client.Models;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain.Model;
using Lykke.Service.IntrinsicEventIndicators.Core.Services;
using Microsoft.AspNetCore.Mvc;
using MoreLinq;

namespace Lykke.Service.IntrinsicEventIndicators.Controllers
{
    [Route("api/v1/[controller]")]
    public class ExternalIntrinsicEventIndicatorsController : Controller, IExternalIntrinsicEventIndicatorsApi
    {
        private readonly IExternalIntrinsicEventIndicatorsService _intrinsicEventIndicatorsService;        

        public ExternalIntrinsicEventIndicatorsController(IExternalIntrinsicEventIndicatorsService intrinsicEventIndicatorsService)
        {
            _intrinsicEventIndicatorsService = intrinsicEventIndicatorsService;
        }

        /// <summary>
        /// Adds delta.
        /// </summary>
        /// <param name="column">Delta to add.</param>
        [HttpPost("intrinsiceventindicatorsdeltaexternal")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task AddDeltaExternalAsync([FromBody] IntrinsicEventIndicatorsColumnPost column)
        {
            column.Delta /= 100;

            var model = Mapper.Map<IntrinsicEventIndicatorsColumn>(column);

            await _intrinsicEventIndicatorsService.AddColumn(model);
        }

        /// <summary>
        /// Deletes delta.
        /// </summary>
        /// <param name="columnId">delta</param>
        /// <returns></returns>
        [HttpDelete("intrinsiceventindicatorsdeltaexternal")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task RemoveDeltaExternalAsync(string columnId)
        {
            if (string.IsNullOrEmpty(columnId))
            {
                throw new ValidationApiException($"{nameof(columnId)} required");
            }

            await _intrinsicEventIndicatorsService.RemoveColumn(columnId);
        }

        /// <summary>
        /// Adds asset pair.
        /// </summary>
        /// <param name="row">Asset pair to add.</param>
        [HttpPost("intrinsiceventindicatorsassetpairexternal")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task AddAssetPairExternalAsync([FromBody] IntrinsicEventIndicatorsRowPost row)
        {
            var model = Mapper.Map<IntrinsicEventIndicatorsRow>(row);

            await _intrinsicEventIndicatorsService.AddAssetPair(model);
        }

        /// <summary>
        /// Edits asset pair.
        /// </summary>
        /// <param name="row">Asset pair to edit.</param>
        [HttpPut("intrinsiceventindicatorsassetpairexternal")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task EditAssetPairExternalAsync([FromBody] IntrinsicEventIndicatorsRowEdit row)
        {
            var model = Mapper.Map<IntrinsicEventIndicatorsRow>(row);

            await _intrinsicEventIndicatorsService.EditAssetPair(model);
        }

        /// <summary>
        /// Deletes asset pair.
        /// </summary>
        /// <param name="rowId">asset pair</param>
        /// <returns></returns>
        [HttpDelete("intrinsiceventindicatorsassetpairexternal")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task RemoveAssetPairExternalAsync(string rowId)
        {
            if (string.IsNullOrEmpty(rowId))
            {
                throw new ValidationApiException($"{nameof(rowId)} required");
            }

            await _intrinsicEventIndicatorsService.RemoveAssetPair(rowId);
        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <returns>Data</returns>
        [HttpGet("intrinsiceventindicatorsdataexternal")]
        [ProducesResponseType(typeof(IntrinsicEventIndicatorsDto), (int)HttpStatusCode.OK)]
        public async Task<IntrinsicEventIndicatorsDto> GetDataExternalAsync()
        {
            var data = await _intrinsicEventIndicatorsService.GetData();
            var model = Mapper.Map<Core.Domain.Model.IntrinsicEventIndicators, IntrinsicEventIndicatorsDto>(data);
            model.Columns.ForEach(x => x.Delta *= 100);
            return model;
        }

        /// <summary>
        /// Gets runners states.
        /// </summary>
        /// <returns>Runners states</returns>
        [HttpGet("intrinsiceventindicatorsrunnersstatesexternal")]
        [ProducesResponseType(typeof(IDictionary<string, IList<RunnerStateDto>>), (int)HttpStatusCode.OK)]
        public async Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesExternalAsync()
        {
            var runnersStates = await _intrinsicEventIndicatorsService.GetRunnersStates();
            var model = Mapper.Map<IDictionary<string, IList<IRunnerState>>, IDictionary<string, IList<RunnerStateDto>>>(runnersStates);
            return model;
        }

        /// <summary>
        /// Gets matrix history stamps.
        /// </summary>
        /// <returns>Matrix history stamps</returns>
        [HttpGet("matrixhistorystampsexternal")]
        [ProducesResponseType(typeof(IList<DateTime>), (int)HttpStatusCode.OK)]
        public async Task<IList<DateTime>> GetMatrixHistoryStampsExternalAsync(DateTime date)
        {
            var model = await _intrinsicEventIndicatorsService.GetMatrixHistoryStamps(date);
            return model;
        }

        /// <summary>
        /// Gets matrix history data.
        /// </summary>
        /// <returns>Matrix history data</returns>
        [HttpGet("matrixhistorydataexternal")]
        [ProducesResponseType(typeof(IntrinsicEventIndicatorsDto), (int)HttpStatusCode.OK)]
        public async Task<IntrinsicEventIndicatorsDto> GetMatrixHistoryDataExternalAsync(DateTime date)
        {
            date = date.ToUniversalTime();

            IntrinsicEventIndicatorsDto model = null;
            var data = await _intrinsicEventIndicatorsService.GetMatrixHistoryData(date);
            if (data != null)
            {
                model = Mapper.Map<Core.Domain.Model.IntrinsicEventIndicators, IntrinsicEventIndicatorsDto>(data);
                model.Columns.ForEach(x => x.Delta *= 100);
            }

            return model;
        }

        /// <summary>
        /// Gets event history data.
        /// </summary>
        /// <returns>Event history data</returns>
        [HttpGet("eventhistorydataexternal")]
        [ProducesResponseType(typeof(IReadOnlyList<EventHistoryDto>), (int)HttpStatusCode.OK)]
        public async Task<IReadOnlyList<EventHistoryDto>> GetEventHistoryDataExternalAsync(DateTime date, string exchange, string assetPair, decimal? delta)
        {
            var data = await _intrinsicEventIndicatorsService.GetEventHistoryData(date, exchange, assetPair, delta);
            var model = Mapper.Map<IReadOnlyList<IEventHistory>, IReadOnlyList<EventHistoryDto>>(data);
            return model;
        }
    }
}
