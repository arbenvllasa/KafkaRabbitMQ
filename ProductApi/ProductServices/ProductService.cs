using Confluent.Kafka;
using Microsoft.VisualBasic;
using ProductApi.Data;
using Shared;
using System.Text.Json;

namespace ProductApi.ProductServices
{

    public class ProductService : IProductService
    {

        private readonly AppDbContext _appDbContext;

        private readonly IProducer<Null,string> _producer;
        

        public ProductService(AppDbContext appDbContext, IProducer<Null,string> producer)
        {
            _appDbContext = appDbContext;
            _producer = producer;

        }


    


        public async Task AddProduct(Product product)
        {
         _appDbContext.Products.Add(product);
            await _appDbContext.SaveChangesAsync();
            var result = await _producer.ProduceAsync("add-product-sqltopic",
                new Message<Null, string>
                {
                    Value = JsonSerializer.Serialize(product)
                });

            if(result.Status!=PersistenceStatus.Persisted)
            {
                _appDbContext.Products.Remove(product);
                await _appDbContext.SaveChangesAsync();
                
            }


        }

        public async Task DeleteProduct(int id)
        {
           var product=await _appDbContext.Products.FindAsync(id);
            if (product != null) 
            {
                _appDbContext.Products.Remove(product);
                await _appDbContext.SaveChangesAsync();

                await _producer.ProduceAsync("delte_sql_topic", new Message<Null, string>
                {
                    Value = id.ToString()
                });

                
            }



        }

        public async Task<Product?> UpdateProductPrice(int id, decimal newPrice)
        {
            // Find the product by ID
            var product = await _appDbContext.Products.FindAsync(id);

            if (product == null)
            {
                // Return null if the product was not found
                return null;
            }

            // Update the product's price
            product.Price = newPrice;
            await _appDbContext.SaveChangesAsync();

            // Send a Kafka message with the updated price
            var message = new { ProductId = id, NewPrice = newPrice };
            await _producer.ProduceAsync("update-price", new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(message)
            });

            // Return the updated product
            return product;
        }
    }
}
