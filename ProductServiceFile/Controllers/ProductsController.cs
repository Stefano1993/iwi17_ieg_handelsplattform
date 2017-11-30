using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ProductServiceFile.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        // GET api/Products
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/Products/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
