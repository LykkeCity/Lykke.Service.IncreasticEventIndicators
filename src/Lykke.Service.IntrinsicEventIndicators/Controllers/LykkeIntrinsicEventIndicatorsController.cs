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

namespace Lykke.Service.IntrinsicEventIndicators.Controllers
{
    [Route("api/v1/[controller]")]
    public class LykkeIntrinsicEventIndicatorsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILykkeIntrinsicEventIndicatorsService _intrinsicEventIndicatorsService;        

        public LykkeIntrinsicEventIndicatorsController(IMapper mapper, ILykkeIntrinsicEventIndicatorsService intrinsicEventIndicatorsService)
        {
            _mapper = mapper;
            _intrinsicEventIndicatorsService = intrinsicEventIndicatorsService;
        }

        /// <summary>
        /// Adds delta.
        /// </summary>
        /// <param name="column">Delta to add.</param>
        [HttpPut("intrinsiceventindicatorsdelta")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddDelta([FromBody] IntrinsicEventIndicatorsColumnPost column)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

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
        [HttpDelete("intrinsiceventindicatorsdelta")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveDelta(string columnId)
        {
            await _intrinsicEventIndicatorsService.RemoveColumn(columnId);
            return NoContent();
        }

        /// <summary>
        /// Adds asset pair.
        /// </summary>
        /// <param name="row">Asset pair to add.</param>
        [HttpPut("intrinsiceventindicatorsassetpair")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAssetPair([FromBody] IntrinsicEventIndicatorsRowPost row)
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
        /// Deletes asset pair.
        /// </summary>
        /// <param name="rowId">asset pair</param>
        /// <returns></returns>
        [HttpDelete("intrinsiceventindicatorsassetpair")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveAssetPair(string rowId)
        {
            await _intrinsicEventIndicatorsService.RemoveAssetPair(rowId);
            return NoContent();
        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <returns>Data</returns>
        [HttpGet("intrinsiceventindicatorsdata", Name = "GetData")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IntrinsicEventIndicatorsDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetData()
        {
            var data = await _intrinsicEventIndicatorsService.GetData();
            var vm = _mapper.Map<IntrinsicEventIndicators.Core.Domain.Model.IntrinsicEventIndicators, IntrinsicEventIndicatorsDto>(data);
            return Ok(vm);
        }

        /// <summary>
        /// Gets runners states.
        /// </summary>
        /// <returns>Runners states</returns>
        [HttpGet("intrinsiceventindicatorsrunnersstates", Name = "GetRunnersStates")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IDictionary<string, IList<RunnerStateDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRunnersStates()
        {
            var runnersStates = await _intrinsicEventIndicatorsService.GetRunnersStates();
            var vm = _mapper.Map<IDictionary<string, IList<IRunnerState>>, IDictionary<string, IList<RunnerStateDto>>>(runnersStates);
            return Ok(vm);
        }
    }
}
