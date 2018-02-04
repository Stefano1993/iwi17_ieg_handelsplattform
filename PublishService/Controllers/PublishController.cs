using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace PublishService.Controllers
{
    [Route("api/[controller]")]
    public class PublishController : Controller
    {
        [HttpGet("{id}")]
        [Produces("application/xml")]
        public async Task<IActionResult> GetAsync(int id, String username, String pw)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage _responseToken =client.GetAsync("http://iwi-mostwanted-secret.azurewebsites.net/api/secret/token?username=" + username+"&pw="+pw).Result;
            if (!_responseToken.StatusCode.ToString().Equals("OK")) return BadRequest("Username/PW falsch");
            String token = _responseToken.Content.ReadAsStringAsync().Result;

            client.DefaultRequestHeaders.Add("Authorization", token);
            HttpResponseMessage _response = client.GetAsync("http://iwi-mostwanted-evaluation.azurewebsites.net/api/eval/" + id).Result;
            String ResponseCode = _response.StatusCode.ToString();
            if (ResponseCode.Equals("NotFound"))
            {
                return BadRequest("Auswertung nicht gefunden");
            }
            else if (ResponseCode.Equals("BadRequest"))
            {
                return BadRequest("Keine Berechtigung");
            }
            else
            {
                EvalResult _erg = JsonConvert.DeserializeObject<EvalResult>(_response.Content.ReadAsStringAsync().Result);
                return Ok(_erg);
            }
           
        }
    }
}
