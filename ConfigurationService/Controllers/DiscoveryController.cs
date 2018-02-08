using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ConfigurationService.Model;
using System.Linq;
using ConfigurationService.Helper;

namespace ConfigurationService.Controllers
{
    [Route("api/Discovery")]
    public class DiscoveryController : Controller
    {
        // GET: api/Discovery
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await HealthChecker.Instance.Check();
            return Ok(HealthChecker.Instance.Services);
        }

        // GET api/Discovery/{serviceName}
        [HttpGet("{serviceName}")]
        public async Task<IActionResult> Get(string serviceName)
        {
            await HealthChecker.Instance.Check(serviceName);
            Service service = HealthChecker.Instance.Services.FirstOrDefault(s => s.ServiceName == serviceName && s.Healthy);
            if(service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        // POST api/Discovery
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ServiceSubscription value)
        {
            Service service = new Service
            {
                ServiceName = value.ServiceName,
                BaseUrl = value.BaseUrl,
                HealthCheckRoute = value.HealthCheckRoute,
                Healthy = false
            };
            HealthChecker.Instance.Services.Add(service);
            return Ok();
        }
    }
}
