namespace BackEnd.CouponAPI.Models.DTO
{
    public class CouponDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; } = "";
        public double Discount { get; set; }
        public double MinAmount { get; set; }

        public static explicit operator Coupon(CouponDTO couponDto)
        {
            return new Coupon
            {
                Id = couponDto.Id,
                Code = couponDto.Code,
                Discount = couponDto.Discount,
                MinAmount = couponDto.MinAmount
            };
        }
    }
}
