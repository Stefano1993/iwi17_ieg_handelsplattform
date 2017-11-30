using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ProductServiceLocal.Controllers
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

        // POST api/Products
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/Products/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/Products/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
