namespace Web.Models
{
    public class OrderDetailsDTO
    {
        public string OrderDetailsId { get; set; }
        public string OrderHeaderId { get; set; }
        public string ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
