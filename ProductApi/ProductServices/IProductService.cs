using Confluent.Kafka;
using Shared;
using System.Text.Json;

namespace ProductApi.ProductServices
{
    public interface IProductService
    {
        Task AddProduct(Product product);
        Task DeleteProduct(int id);
        Task<Product?> UpdateProductPrice(int id, decimal newPrice);




    }

   
}
