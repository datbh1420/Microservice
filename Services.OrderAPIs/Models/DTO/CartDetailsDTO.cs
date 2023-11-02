namespace Services.OrderAPI.Models.DTO
{
    public class CartDetailsDTO
    {
        public string CartDetailsId { get; set; }
        public string CartHeaderId { get; set; }
        public CartHeaderDTO? CartHeader { get; set; }
        public string ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Count { get; set; }

    }
}
