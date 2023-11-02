using BackEnd.CartAPI.Models;
using BackEnd.CartAPI.Models.DTO;

namespace Services.CartAPI.Models.DTO
{
    public class CartDetailsDTO
    {
        public string CartDetailsId { get; set; } = Guid.NewGuid().ToString();
        public string? CartHeaderId { get; set; }
        public string? ProductId { get; set; }
        public int Count { get; set; }
        public CartHeaderDTO? CartHeader { get; set; }
        public ProductDTO? Product { get; set; }

        public static explicit operator CartDetails(CartDetailsDTO cartDetails)
        {
            return new CartDetails
            {
                CartDetailsId = cartDetails.CartDetailsId,
                CartHeaderId = cartDetails.CartHeaderId,
                ProductId = cartDetails.ProductId,
                Product = cartDetails.Product,
                Count = cartDetails.Count,
            };
        }
    }
}
