using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
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
        [HttpPut("intrinsiceventindicatorsdelta")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task AddDelta([FromBody] IntrinsicEventIndicatorsColumnPost column)
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
        [HttpDelete("intrinsiceventindicatorsdelta")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task RemoveDelta(string columnId)
        {
            await _intrinsicEventIndicatorsService.RemoveColumn(columnId);
        }

        /// <summary>
        /// Adds asset pair.
        /// </summary>
        /// <param name="row">Asset pair to add.</param>
        [HttpPut("intrinsiceventindicatorsassetpair")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task AddAssetPair([FromBody] IntrinsicEventIndicatorsRowPost row)
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
        [HttpPost("intrinsiceventindicatorsassetpair")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task EditAssetPair([FromBody] IntrinsicEventIndicatorsRowEdit row)
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
        [HttpDelete("intrinsiceventindicatorsassetpair")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task RemoveAssetPair(string rowId)
        {
            await _intrinsicEventIndicatorsService.RemoveAssetPair(rowId);
        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <returns>Data</returns>
        [HttpGet("intrinsiceventindicatorsdata", Name = "GetData")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IntrinsicEventIndicatorsDto), (int)HttpStatusCode.OK)]
        public async Task<IntrinsicEventIndicatorsDto> GetData()
        {
            var data = await _intrinsicEventIndicatorsService.GetData();
            var model = Mapper.Map<IntrinsicEventIndicators.Core.Domain.Model.IntrinsicEventIndicators, IntrinsicEventIndicatorsDto>(data);
            model.Columns.ForEach(x => x.Delta *= 100);
            return model;
        }

        /// <summary>
        /// Gets runners states.
        /// </summary>
        /// <returns>Runners states</returns>
        [HttpGet("intrinsiceventindicatorsrunnersstates", Name = "GetRunnersStates")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IDictionary<string, IList<RunnerStateDto>>), (int)HttpStatusCode.OK)]
        public async Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStates()
        {
            var runnersStates = await _intrinsicEventIndicatorsService.GetRunnersStates();
            var model = Mapper.Map<IDictionary<string, IList<IRunnerState>>, IDictionary<string, IList<RunnerStateDto>>>(runnersStates);
            return model;
        }
    }
}
