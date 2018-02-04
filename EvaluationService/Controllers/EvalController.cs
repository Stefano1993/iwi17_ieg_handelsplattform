using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvaluationService.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace EvaluationService.Controllers
{
    [Route("api/[controller]")]
    public class EvalController : Controller
    {
        private static EvalBI _evalBI;

        public EvalController()
        {
            _evalBI = new EvalBI();
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromHeader] string Authorization, int id)
        {
            if (Authorization == null) return Forbid("Keine Ahtorisierungsinformationen");    

            EvalResult result = await _evalBI.GetEvalById(id);
            if (result != null)
            {
                if (result.OwnerCredentials.Equals(Authorization)) return Ok(result);
                return BadRequest("Nicht Authorisiert");
            }
            return NotFound();
        }

       
    }
}
