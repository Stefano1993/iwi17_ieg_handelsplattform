using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ConfigurationService.Model;
using System.Threading.Tasks;
using System.Linq;

namespace ConfigurationService.Controllers
{
    [Route("api/Configuration")]
    public class ConfigurationController : Controller
    {
        private static List<Configuration> _configurations;

        public ConfigurationController()
        {
            _configurations = new List<Configuration>
            {
                new Configuration
                {
                    ServiceName = "StorageService",
                    Configurations = new Dictionary<string, string>
                    {
                        { "DatabaseConnectionString", "localhost;initial catalog=testdb;just a sample" }
                    }
                },
                new Configuration
                {
                    ServiceName = "CreationService",
                    Configurations = new Dictionary<string, string>
                    {
                        { "PersonAge", "20" },
                        { "Prio1PaymentMethod", "Credit Card" },
                        { "Prio2PaymentMethod", "Debit Card" },
                        { "Prio3PaymentMethod", "Campus02 token" },
                        { "FirstName", "Max" },
                        { "LastName", "Mustermann" }
                    }
                }
            };
        }

        // GET api/Configuration/{serviceName}
        [HttpGet]
        [Route("{serviceName}")]
        public async Task<IActionResult> Get(string serviceName)
        {
            Configuration config = _configurations.SingleOrDefault(c => c.ServiceName == serviceName);
            if(config == null)
            {
                return NotFound();
            }
            return Ok(config);
        }
    }
}
