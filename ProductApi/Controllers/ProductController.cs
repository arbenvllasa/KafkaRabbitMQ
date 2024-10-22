
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

        [HttpPut("{id:int}/{price:decimal}")]
        public async Task<IActionResult>UpdatePrice(int id, decimal price)
        {
            try
            {
                // Update the product price
                var updatedProduct = await productService.UpdateProductPrice(id, price);

                if (updatedProduct == null)
                {
                    // Return a NotFound result if the product does not exist
                    return NotFound(new { message = $"Product with ID {id} not found." });
                }

                // Return the updated product
                return Ok(new { message = "Price updated successfully", product = updatedProduct });
            }
            catch (Exception ex)
            {
                // Return an error response if something goes wrong
                return StatusCode(500, new { message = "An error occurred while updating the price", error = ex.Message });
            }

        }
    }
}
