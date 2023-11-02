using BackEnd.ProductAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.ProductAPI.Models
{
    public class Product
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; } = "";
        [Range(1, 1000)]
        public double Price { get; set; }
        [Range(0, 10000)]
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public string CategoryName { get; set; } = "";
        public string? ImagePath { get; set; }
        public string? ImageLocalPath { get; set; }

        public static explicit operator ProductDTO(Product productDto)
        {
            return new ProductDTO
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                Description = productDto.Description,
                CategoryName = productDto.CategoryName,
                ImagePath = productDto.ImagePath,
                ImageLocalPath = productDto.ImageLocalPath,
            };
        }
    }
}
