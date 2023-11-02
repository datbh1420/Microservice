namespace BackEnd.ProductAPI.Models.DTO
{
    public class ProductDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public string CategoryName { get; set; } = "";
        public string? ImagePath { get; set; }
        public string? ImageLocalPath { get; set; }
        public IFormFile? Image { get; set; }

        public static explicit operator Product(ProductDTO product)
        {
            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                CategoryName = product.CategoryName,
                ImagePath = product.ImagePath,
                ImageLocalPath = product.ImageLocalPath,
            };
        }
    }
}
