namespace BackEnd.CartAPI.Models.DTO
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
    }
}
