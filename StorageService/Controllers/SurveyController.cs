using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorageService.Model;

namespace StorageService.Controllers
{
    [Route("api/Survey")]
    public class SurveyController : Controller
    {
        private static List<Survey> _surveys = new List<Survey>();

        // POST api/Survey
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Survey value)
        {
            _surveys.Add(value);
            return Ok();
        }

        // GET api/Survey
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(_surveys);
        }
    }
}
