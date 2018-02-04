using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretService.BuinessLogik;
using SecretService.Models;

namespace SecretService.Controllers
{
    [Route("api/[controller]")]
    public class SecretController : Controller
    {
        private static SecretManager secretManager;

        public SecretController()
        {
            secretManager = new SecretManager();
        }

        [HttpGet("token")]
        public async Task<IActionResult> Get(string username, string pw)
        {
            Token token = secretManager.findToken(username, pw);
            if (token.Tokenstring == null) return BadRequest("Username/PW falsch");
            return Ok(token.Tokenstring.ToString());
        }

       
    }
}
