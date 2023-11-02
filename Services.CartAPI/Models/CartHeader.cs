using BackEnd.CartAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.CartAPI.Models
{
    public class CartHeader
    {
        [Key]
        public string CartHeaderId { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public string? Code { get; set; }
        [NotMapped]
        public double Discount { get; set; }
        [NotMapped]
        public double CartTotal { get; set; }

        public static explicit operator CartHeaderDTO(CartHeader cartHeader)
        {
            return new CartHeaderDTO
            {
                CartHeaderId = cartHeader.CartHeaderId,
                UserId = cartHeader.UserId,
                Code = cartHeader.Code,
                Discount = cartHeader.Discount,
                CartTotal = cartHeader.CartTotal
            };
        }
    }
}
