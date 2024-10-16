using Shared;
using System.Text.Json;
using Confluent.Kafka;


namespace OrderApi.OrderServices
{
    public class OrderService(IConsumer<Null, string> consumer) : IOrderService
    {
        private const string AddProductTopic = "add-product-topic";
        private const string DeleteProductTopic = "delete-product-topic";

        public List<Product> Products=[];

        public List<Order> Orders = [];



        public async Task StartConsumingServices()
        {
            await Task.Delay(10);

            consumer.Subscribe([AddProductTopic, DeleteProductTopic]);


            while (true)
            {
                var response = consumer.Consume();
                if (!string.IsNullOrEmpty(response.Message.Value))
                {
                    //check if topic== add product topic
                    if (response.Topic == AddProductTopic)
                    {
                        var product = JsonSerializer.Deserialize<Product>(response.Message.Value);
                        Products.Add(product!);

                    }
                    else
                    {

                        Products.Remove(Products.
                            FirstOrDefault(p => p.Id == int.Parse(response.Message.Value))!);


                    }

                    ConsoleProduct();


                }
            }

        }


        public void AddOrder(Order order) => Orders.Add(order);



        public List<OrderSummery> GetOrdersSummery()
        {
            var orderSummery = new List<OrderSummery>();
            foreach (var order in Orders)

                orderSummery.Add(new OrderSummery()
                {
                    OrderId = order.Id,
                    OrderQuantity = order.Quantity,
                    ProductId = order.ProductID,
                    ProductName = Products.FirstOrDefault(p => p.Id == order.ProductID)!.Name,
                    ProductPrice = Products.FirstOrDefault(p => p.Id == order.ProductID)!.Price,


                });

            return orderSummery;



        }

        public List<Product> GetProducts() => Products;


       

        private void ConsoleProduct()
        {
            Console.Clear();
            foreach (var item in Products)
                Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Price: {item.Price}");


        }

    }
}
