using Confluent.Kafka;
using Shared;
using System.Text.Json;

namespace OrderApi.OrderServices
{
    public interface IOrderService
    {
        Task StartConsumingServices();
        void AddOrder(Order order);

        List<Product> GetProducts();

        List<OrderSummery> GetOrdersSummery();




    }
   
}
