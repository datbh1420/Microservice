using Services.OrderAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace Services.OrderAPI.Models
{
    public class OrderHeader
    {
        [Key]
        public string OrderHeaderId { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public string? Code { get; set; }
        public double Discount { get; set; }
        public double OrderTotal { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? OrderTime { get; set; }
        public string? Status { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? StripeSessionId { get; set; }
        public IEnumerable<OrderDetail>? OrderDetails { get; set; }

        public static implicit operator OrderHeaderDTO(OrderHeader orderHeader)
        {
            return new OrderHeaderDTO
            {
                OrderHeaderId = orderHeader.OrderHeaderId,
                UserId = orderHeader.UserId,
                Code = orderHeader.Code,
                Discount = orderHeader.Discount,
                OrderTotal = orderHeader.OrderTotal,
                Name = orderHeader.Name,
                Email = orderHeader.Email,
                Phone = orderHeader.Phone,
                OrderTime = orderHeader.OrderTime,
                Status = orderHeader.Status,
                PaymentIntentId = orderHeader.PaymentIntentId,
                StripeSessionId = orderHeader.StripeSessionId,
                OrderDetails = orderHeader.OrderDetails.Select(x => (OrderDetailsDTO)x)
            };
        }
    }
}
