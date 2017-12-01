using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductServiceFile.BusinessLogic;
using ProductServiceFile.Models;
using System.Threading.Tasks;

namespace ProductServiceFile.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private static ProductBl _productBl;
        private IConfiguration _configuration;

        public ProductsController(IConfiguration configuration)
        {
            _productBl = new ProductBl(configuration);
        }

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
    }
}
