using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
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
    public class LykkeIntrinsicEventIndicatorsController : Controller, ILykkeIntrinsicEventIndicatorsApi
    {
        private readonly ILykkeIntrinsicEventIndicatorsService _intrinsicEventIndicatorsService;        

        public LykkeIntrinsicEventIndicatorsController(ILykkeIntrinsicEventIndicatorsService intrinsicEventIndicatorsService)
        {
            _intrinsicEventIndicatorsService = intrinsicEventIndicatorsService;
        }

        /// <summary>
        /// Adds delta.
        /// </summary>
        /// <param name="column">Delta to add.</param>
        [HttpPost("intrinsiceventindicatorsdelta")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task AddDeltaAsync([FromBody] IntrinsicEventIndicatorsColumnPost column)
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
        [HttpDelete("intrinsiceventindicatorsdelta")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task RemoveDeltaAsync(string columnId)
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
        [HttpPost("intrinsiceventindicatorsassetpair")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task AddAssetPairAsync([FromBody] [CustomizeValidator(RuleSet = "lykke")] IntrinsicEventIndicatorsRowPost row)
        {
            var model = Mapper.Map<IntrinsicEventIndicatorsRow>(row);

            await _intrinsicEventIndicatorsService.AddAssetPair(model);
        }

        /// <summary>
        /// Edits asset pair.
        /// </summary>
        /// <param name="row">Asset pair to edit.</param>
        [HttpPut("intrinsiceventindicatorsassetpair")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task EditAssetPairAsync([FromBody] IntrinsicEventIndicatorsRowEdit row)
        {
            var model = Mapper.Map<IntrinsicEventIndicatorsRow>(row);

            await _intrinsicEventIndicatorsService.EditAssetPair(model);
        }

        /// <summary>
        /// Deletes asset pair.
        /// </summary>
        /// <param name="rowId">asset pair</param>
        /// <returns></returns>
        [HttpDelete("intrinsiceventindicatorsassetpair")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task RemoveAssetPairAsync(string rowId)
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
        [HttpGet("intrinsiceventindicatorsdata")]
        [ProducesResponseType(typeof(IntrinsicEventIndicatorsDto), (int)HttpStatusCode.OK)]
        public async Task<IntrinsicEventIndicatorsDto> GetDataAsync()
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
        [HttpGet("intrinsiceventindicatorsrunnersstates")]
        [ProducesResponseType(typeof(IDictionary<string, IList<RunnerStateDto>>), (int)HttpStatusCode.OK)]
        public async Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesAsync()
        {
            var runnersStates = await _intrinsicEventIndicatorsService.GetRunnersStates();
            var model = Mapper.Map<IDictionary<string, IList<IRunnerState>>, IDictionary<string, IList<RunnerStateDto>>>(runnersStates);
            return model;
        }

        /// <summary>
        /// Gets matrix history stamps.
        /// </summary>
        /// <returns>Matrix history stamps</returns>
        [HttpGet("matrixhistorystamps")]
        [ProducesResponseType(typeof(IList<DateTime>), (int)HttpStatusCode.OK)]
        public async Task<IList<DateTime>> GetMatrixHistoryStampsAsync(DateTime date)
        {
            var model = await _intrinsicEventIndicatorsService.GetMatrixHistoryStamps(date);
            return model;
        }

        /// <summary>
        /// Gets matrix history data.
        /// </summary>
        /// <returns>Matrix history data</returns>
        [HttpGet("matrixhistorydata")]
        [ProducesResponseType(typeof(IntrinsicEventIndicatorsDto), (int)HttpStatusCode.OK)]
        public async Task<IntrinsicEventIndicatorsDto> GetMatrixHistoryDataAsync(DateTime date)
        {
            date = date.ToUniversalTime(); //date = new DateTime(date.Ticks, DateTimeKind.Utc);

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
        [HttpGet("eventhistorydata")]
        [ProducesResponseType(typeof(IReadOnlyList<EventHistoryDto>), (int)HttpStatusCode.OK)]
        public async Task<IReadOnlyList<EventHistoryDto>> GetEventHistoryDataAsync(DateTime from, DateTime to, string exchange, string assetPair, decimal? delta)
        {
            var data = await _intrinsicEventIndicatorsService.GetEventHistoryData(from, to, exchange, assetPair, delta);
            var model = Mapper.Map<IReadOnlyList<IEventHistory>, IReadOnlyList<EventHistoryDto>>(data);
            return model;
        }
    }
}
