using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.IncreasticEventIndicators.Core.Services.Exchanges;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.IncreasticEventIndicators.Controllers
{
    [Route("api/v1/[controller]")]
    public class IntrinsicEventIndicatorsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILykkeTickPriceHandler _lykkeTickPriceHandler;

        public IntrinsicEventIndicatorsController(IMapper mapper, ILykkeTickPriceHandler lykkeTickPriceHandler)
        {
            _mapper = mapper;
            _lykkeTickPriceHandler = lykkeTickPriceHandler;
        }

        /// <summary>
        /// Adds delta.
        /// </summary>
        /// <param name="delta">Delta to add.</param>
        [HttpPut("intrinsiceventindicatorsdelta")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddDelta()
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ErrorResponse.Create(ModelState));
            //}

            //var model = _mapper
            //    .Map<ScenarioAnalysisMatrixColumn>(column);

            try
            {
                //await _scenarioAnalysisMatrixService.AddColumn(instanceName, model);
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
        /// <param name="delta">delta</param>
        /// <returns></returns>
        [HttpDelete("intrinsiceventindicatorsdelta")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveDelta()
        {
            //await _scenarioAnalysisMatrixService.RemoveColumn(instanceName, columnId);
            return NoContent();
        }
    }
}
