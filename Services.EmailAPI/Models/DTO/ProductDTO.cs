﻿using System.ComponentModel.DataAnnotations;

namespace Services.EmailAPI.Models.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        [Range(1, 1000)]
        public int Count { get; set; } = 1;
    }
}
