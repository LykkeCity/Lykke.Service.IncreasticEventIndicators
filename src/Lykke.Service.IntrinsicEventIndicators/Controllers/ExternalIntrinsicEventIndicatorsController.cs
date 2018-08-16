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
        [HttpPut("intrinsiceventindicatorsdeltaexternal")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task AddDeltaExternal([FromBody] IntrinsicEventIndicatorsColumnPost column)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationApiException("Invalid model");
            }

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
        public async Task RemoveDeltaExternal(string columnId)
        {
            await _intrinsicEventIndicatorsService.RemoveColumn(columnId);
        }

        /// <summary>
        /// Adds asset pair.
        /// </summary>
        /// <param name="row">Asset pair to add.</param>
        [HttpPut("intrinsiceventindicatorsassetpairexternal")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task AddAssetPairExternal([FromBody] IntrinsicEventIndicatorsRowPost row)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationApiException("Invalid model");
            }

            var model = Mapper.Map<IntrinsicEventIndicatorsRow>(row);

            await _intrinsicEventIndicatorsService.AddAssetPair(model);
        }

        /// <summary>
        /// Edits asset pair.
        /// </summary>
        /// <param name="row">Asset pair to edit.</param>
        [HttpPost("intrinsiceventindicatorsassetpairexternal")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task EditAssetPairExternal([FromBody] IntrinsicEventIndicatorsRowEdit row)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationApiException("Invalid model");
            }

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
        public async Task RemoveAssetPairExternal(string rowId)
        {
            await _intrinsicEventIndicatorsService.RemoveAssetPair(rowId);
        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <returns>Data</returns>
        [HttpGet("intrinsiceventindicatorsdataexternal", Name = "GetDataExternal")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IntrinsicEventIndicatorsDto), (int)HttpStatusCode.OK)]
        public async Task<IntrinsicEventIndicatorsDto> GetDataExternal()
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
        [HttpGet("intrinsiceventindicatorsrunnersstatesexternal", Name = "GetRunnersStatesExternal")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IDictionary<string, IList<RunnerStateDto>>), (int)HttpStatusCode.OK)]
        public async Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesExternal()
        {
            var runnersStates = await _intrinsicEventIndicatorsService.GetRunnersStates();
            var model = Mapper.Map<IDictionary<string, IList<IRunnerState>>, IDictionary<string, IList<RunnerStateDto>>>(runnersStates);
            return model;
        }
    }
}
