using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.IntrinsicEventIndicators.Client.Api;
using Lykke.Service.IntrinsicEventIndicators.Client.Models;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain.Model;
using Lykke.Service.IntrinsicEventIndicators.Core.Services;
using Microsoft.AspNetCore.Mvc;
using MoreLinq;

namespace Lykke.Service.IntrinsicEventIndicators.Controllers
{
    [Route("api/[controller]")]
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
        [HttpPost("delta")]
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
        [HttpDelete("delta")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task RemoveDeltaAsync(string columnId)
        {
            await _intrinsicEventIndicatorsService.RemoveColumn(columnId);
        }

        /// <summary>
        /// Adds asset pair.
        /// </summary>
        /// <param name="row">Asset pair to add.</param>
        [HttpPost("assetpair")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task AddAssetPairAsync([FromBody] IntrinsicEventIndicatorsRowPost row)
        {
            var model = Mapper.Map<IntrinsicEventIndicatorsRow>(row);

            await _intrinsicEventIndicatorsService.AddAssetPair(model);
        }

        /// <summary>
        /// Edits asset pair.
        /// </summary>
        /// <param name="row">Asset pair to edit.</param>
        [HttpPut("assetpair")]
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
        [HttpDelete("assetpair")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task RemoveAssetPairAsync(string rowId)
        {
            await _intrinsicEventIndicatorsService.RemoveAssetPair(rowId);
        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <returns>Data</returns>
        [HttpGet("data")]
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
        [HttpGet("runnersstates")]
        [ProducesResponseType(typeof(IDictionary<string, IList<RunnerStateDto>>), (int)HttpStatusCode.OK)]
        public async Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesAsync()
        {
            var runnersStates = await _intrinsicEventIndicatorsService.GetRunnersStates();
            var model = Mapper.Map<IDictionary<string, IList<IRunnerState>>, IDictionary<string, IList<RunnerStateDto>>>(runnersStates);
            return model;
        }
    }
}
