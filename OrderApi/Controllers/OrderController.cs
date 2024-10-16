using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.OrderServices;
using Shared;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        [HttpGet("start-consuming-services")]
        public async Task<IActionResult> StartService()
        {
            await orderService.StartConsumingServices();
            return NoContent();

        }

        [HttpGet("get-product")]
        public IActionResult GetProduct()
        {
            var product=orderService.GetProducts();
            return Ok(product);

        }

        [HttpPost("add-order")]
        public IActionResult AddOrder(Order order)
        {
            orderService.AddOrder(order);
            return Ok("Order Created");

        }
        [HttpGet("order-sumery")]
        public IActionResult GetOrderSumerry()=>Ok(orderService.GetOrdersSummery());

        

    }
}
