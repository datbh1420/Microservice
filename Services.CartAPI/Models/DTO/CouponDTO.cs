namespace BackEnd.CartAPI.Models.DTO
{
    public class CouponDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; } = "";
        public double Discount { get; set; }
        public double MinAmount { get; set; }
    }
}
