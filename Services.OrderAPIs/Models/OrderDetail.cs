using Services.OrderAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.OrderAPI.Models
{
    public class OrderDetail
    {
        [Key]
        public string OrderDetailId { get; set; } = Guid.NewGuid().ToString();
        public string? OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader? OrderHeader { get; set; }
        public string? ProductId { get; set; }
        [NotMapped]
        public ProductDTO? Product { get; set; }
        public int Count { get; set; }
        public string? ProductName { get; set; }
        public double Price { get; set; }

        public static implicit operator OrderDetailsDTO(OrderDetail orderDetail)
        {
            return new OrderDetailsDTO
            {
                OrderDetailsId = orderDetail.OrderDetailId,
                OrderHeaderId = orderDetail.OrderHeaderId,
                Count = orderDetail.Count,
                Price = orderDetail.Price,
                Product = orderDetail.Product,
                ProductId = orderDetail.ProductId,
                ProductName = orderDetail.ProductName,
            };
        }
    }
}
