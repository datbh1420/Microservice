using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class CartHeaderDTO
    {
        public string CartHeaderId { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public string? Code { get; set; }
        public double Discount { get; set; }
        public double CartTotal { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Phone { get; set; }
    }
}
