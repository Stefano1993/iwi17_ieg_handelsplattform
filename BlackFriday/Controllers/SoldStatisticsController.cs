using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlackFriday.Interfaces;
using BlackFriday.Dal;
using System.Threading.Tasks;

namespace BlackFriday.Controllers
{
    [Produces("application/json")]
    [Route("api/Statistics")]
    public class SoldStatisticsController : Controller
    {
        private readonly ILogger<CashDeskController> _logger;
        private ISoldStatisticDal _statisticsDal;

        public SoldStatisticsController(ILogger<CashDeskController> logger)
        {
            _logger = logger;
            _statisticsDal = new SoldStatisticDal();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(_statisticsDal.GetSells());
        }
    }
}