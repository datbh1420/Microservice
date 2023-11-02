namespace Web.Models
{
    public class CartDetailsDTO
    {
        public string CartDetailsId { get; set; } = Guid.NewGuid().ToString();
        public string CartHeaderId { get; set; }
        public CartHeaderDTO? CartHeader { get; set; }
        public string ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Count { get; set; }

    }
}
