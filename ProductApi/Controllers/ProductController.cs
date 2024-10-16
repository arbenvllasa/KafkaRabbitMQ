
using Microsoft.AspNetCore.Mvc;
using ProductApi.ProductServices;
using Shared;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult>AddProduct(Product product)
        {
            await productService.AddProduct(product);

            return Created();


        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult>DeleteProduct(int Id)
        {
            await productService.DeleteProduct(Id);
            return NoContent();

        }
    }
}
