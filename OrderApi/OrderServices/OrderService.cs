using Shared;
using System.Text.Json;
using Confluent.Kafka;


namespace OrderApi.OrderServices
{
    public class OrderService(IConsumer<Null, string> consumer) : IOrderService
    {
        private const string AddProductTopic = "add-product-sqltopic";
        private const string DeleteProductTopic = "delte_sql_topic";
        private const string UpdatePriceTopic = "update-price";


        public List<Product> Products=[];

        public List<Order> Orders = [];



        public async Task StartConsumingServices()
        {
            await Task.Delay(10);

            consumer.Subscribe([AddProductTopic, DeleteProductTopic,UpdatePriceTopic]);


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
                    else if (response.Topic == DeleteProductTopic)
                    {
                        Products.Remove(Products.FirstOrDefault(p => p.Id == int.Parse(response.Message.Value))!);
                    }
                    // Check if the topic is 'UpdatePriceTopic'
                    else if (response.Topic == UpdatePriceTopic)
                    {
                        // Deserialize the message to get product ID and new price
                        var updateMessage = JsonSerializer.Deserialize<ProductUpdateMessage>(response.Message.Value);

                        if (updateMessage != null)
                        {
                            // Find the product and update its price
                            var productToUpdate = Products.FirstOrDefault(p => p.Id == updateMessage.ProductId);
                            if (productToUpdate != null)
                            {
                                productToUpdate.Price = updateMessage.NewPrice;
                            }
                        }
                    }

                    ConsoleProduct();


                }
            }

        }
        private void ConsoleProduct()
        {
            Console.Clear();
            foreach (var item in Products)
                Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Price: {item.Price}");


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



        public class ProductUpdateMessage
        {
            public int ProductId { get; set; }
            public decimal NewPrice { get; set; }
        }

    }
}
