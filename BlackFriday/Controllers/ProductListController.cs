using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlackFriday.BusinessLogic;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlackFriday.Controllers
{
    [Route("api/[controller]")]
    public class ProductListController : Controller
    {
        private ProductBl _productBl;
        private IConfiguration _configuration;

        public ProductListController(IConfiguration configuration)
        {
            _productBl = new ProductBl(configuration);
        }

        // GET: http://iegblackfriday.azurewebsites.net/api/productlist
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _productBl.GetProducts());
        }
    }
}
