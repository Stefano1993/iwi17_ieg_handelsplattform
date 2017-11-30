using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductServiceLocal.BusinessLogic;
using ProductServiceLocal.Models;
using System.Threading.Tasks;

namespace ProductServiceLocal.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private static ProductBl _productBl = new ProductBl();

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _productBl.GetProducts());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            Product result = await _productBl.GetProductById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product product)
        {
            int id = await _productBl.CreateProduct(product);
            return CreatedAtAction("Get", new { id = id });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Product product)
        {
            int? resultId = await _productBl.UpdateProductById(product);
            if(resultId != null)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            int? resultId = await _productBl.DeleteProductById(id);
            if (resultId != null)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
