using BackEnd.CartAPI.Models.DTO;
using Services.CartAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.CartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public string CartDetailsId { get; set; } = Guid.NewGuid().ToString();
        public string CartHeaderId { get; set; }
        public string? ProductId { get; set; }
        public int Count { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeader CartHeader { get; set; }
        [NotMapped]
        public ProductDTO? Product { get; set; }

        public static explicit operator CartDetailsDTO(CartDetails cartDetails)
        {
            return new CartDetailsDTO
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
