
namespace Shared
{
    public class OrderSummery
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int OrderQuantity { get; set; }
        public decimal TotalAmount => OrderQuantity * ProductPrice;


    }
}
