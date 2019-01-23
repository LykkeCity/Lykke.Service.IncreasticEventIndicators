using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.IntrinsicEventIndicators.Client.Models;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.LyciAssets;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.IntrinsicEventIndicators.Controllers
{
    [Route("api/v1/[controller]")]
    public class PriceControllerController : Controller
    {
        private readonly IPriceManager _priceManager;

        public PriceControllerController(IPriceManager priceManager)
        {
            _priceManager = priceManager;
        }

        /// <summary>
        /// Gets prices.
        /// </summary>
        /// <returns>Mid Prices</returns>
        [HttpGet("getmidprices")]
        [ProducesResponseType(typeof(List<PriceValue>), (int) HttpStatusCode.OK)]
        public async Task<List<PriceValue>> GetPrices()
        {
            var data = _priceManager.GetPrices();
            return data;
        }
    }
}
