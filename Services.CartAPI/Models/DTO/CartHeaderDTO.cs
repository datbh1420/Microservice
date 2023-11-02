namespace BackEnd.CartAPI.Models.DTO
{
    public class CartHeaderDTO
    {
        public string CartHeaderId { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public string? Code { get; set; }
        public double Discount { get; set; }
        public double CartTotal { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public static implicit operator CartHeader(CartHeaderDTO cartHeader)
        {
            return new CartHeader
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
