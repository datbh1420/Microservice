using BackEnd.CouponAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Code { get; set; } = "";
        public double Discount { get; set; }
        public double MinAmount { get; set; }

        public static explicit operator CouponDTO(Coupon coupon)
        {
            return new CouponDTO
            {
                Id = coupon.Id,
                Code = coupon.Code,
                Discount = coupon.Discount,
                MinAmount = coupon.MinAmount
            };
        }
    }
}
