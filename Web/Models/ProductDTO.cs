using Mango.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public string CategoryName { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageLocalPath { get; set; }
        [Range(1, 1000)]
        public int Count { get; set; } = 1;
        [MaxFileSize(1)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile? Image { get; set; }
    }
}
