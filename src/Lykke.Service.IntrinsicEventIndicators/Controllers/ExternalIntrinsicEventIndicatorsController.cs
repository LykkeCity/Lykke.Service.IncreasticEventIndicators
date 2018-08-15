using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain.Model;
using Lykke.Service.IntrinsicEventIndicators.Core.Services;
using Lykke.Service.IntrinsicEventIndicators.Models;
using Microsoft.AspNetCore.Mvc;
using MoreLinq;

namespace Lykke.Service.IntrinsicEventIndicators.Controllers
{
    [Route("api/v1/[controller]")]
    public class ExternalIntrinsicEventIndicatorsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IExternalIntrinsicEventIndicatorsService _intrinsicEventIndicatorsService;        

        public ExternalIntrinsicEventIndicatorsController(IMapper mapper, IExternalIntrinsicEventIndicatorsService intrinsicEventIndicatorsService)
        {
            _mapper = mapper;
            _intrinsicEventIndicatorsService = intrinsicEventIndicatorsService;
        }

        /// <summary>
        /// Adds delta.
        /// </summary>
        /// <param name="column">Delta to add.</param>
        [HttpPut("intrinsiceventindicatorsdeltaexternal")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddDeltaExternal([FromBody] IntrinsicEventIndicatorsColumnPost column)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            column.Delta /= 100;

            var model = _mapper
                .Map<IntrinsicEventIndicatorsColumn>(column);

            try
            {
                await _intrinsicEventIndicatorsService.AddColumn(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorResponse.Create(ex.Message));
            }

            return StatusCode((int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes delta.
        /// </summary>
        /// <param name="columnId">delta</param>
        /// <returns></returns>
        [HttpDelete("intrinsiceventindicatorsdeltaexternal")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveDeltaExternal(string columnId)
        {
            await _intrinsicEventIndicatorsService.RemoveColumn(columnId);
            return NoContent();
        }

        /// <summary>
        /// Adds asset pair.
        /// </summary>
        /// <param name="row">Asset pair to add.</param>
        [HttpPut("intrinsiceventindicatorsassetpairexternal")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAssetPairExternal([FromBody] IntrinsicEventIndicatorsRowPost row)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            var model = _mapper
                .Map<IntrinsicEventIndicatorsRow>(row);

            try
            {
                await _intrinsicEventIndicatorsService.AddAssetPair(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorResponse.Create(ex.Message));
            }

            return StatusCode((int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Edits asset pair.
        /// </summary>
        /// <param name="row">Asset pair to edit.</param>
        [HttpPost("intrinsiceventindicatorsassetpairexternal")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EditAssetPairExternal([FromBody] IntrinsicEventIndicatorsRowEdit row)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            var model = _mapper
                .Map<IntrinsicEventIndicatorsRow>(row);

            try
            {
                await _intrinsicEventIndicatorsService.EditAssetPair(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorResponse.Create(ex.Message));
            }

            return StatusCode((int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes asset pair.
        /// </summary>
        /// <param name="rowId">asset pair</param>
        /// <returns></returns>
        [HttpDelete("intrinsiceventindicatorsassetpairexternal")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveAssetPairExternal(string rowId)
        {
            await _intrinsicEventIndicatorsService.RemoveAssetPair(rowId);
            return NoContent();
        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <returns>Data</returns>
        [HttpGet("intrinsiceventindicatorsdataexternal", Name = "GetDataExternal")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IntrinsicEventIndicatorsDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDataExternal()
        {
            var data = await _intrinsicEventIndicatorsService.GetData();
            var vm = _mapper.Map<IntrinsicEventIndicators.Core.Domain.Model.IntrinsicEventIndicators, IntrinsicEventIndicatorsDto>(data);
            vm.Columns.ForEach(x => x.Delta *= 100);
            return Ok(vm);
        }

        /// <summary>
        /// Gets runners states.
        /// </summary>
        /// <returns>Runners states</returns>
        [HttpGet("intrinsiceventindicatorsrunnersstatesexternal", Name = "GetRunnersStatesExternal")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IDictionary<string, IList<RunnerStateDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRunnersStatesExternal()
        {
            var runnersStates = await _intrinsicEventIndicatorsService.GetRunnersStates();
            var vm = _mapper.Map<IDictionary<string, IList<IRunnerState>>, IDictionary<string, IList<RunnerStateDto>>>(runnersStates);
            return Ok(vm);
        }
    }
}
