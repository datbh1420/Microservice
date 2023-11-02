using BackEnd.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.ProductAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<Product> products = new List<Product> {
                new Product
                {
                    Id = "c484e233-89c5-4dd0-9759-948a09a92682",
                    Name = "STRESS RELEASING ELASTIC TOYS CUTE CRITIC CHICKENS",
                    Price = 68_000,
                    Quantity = 100,
                    Description = "Tired, tense, stressed,...after every hour of studying, taking exams, working??? Let To relieve your tired feeling with these extremely funny and cute elastic toys!",
                    ImagePath = "https://localhost:7001/Images/CriticChickens.png",
                    ImageLocalPath = @"Images\CriticChickens.png",
                    CategoryName = "Stuffed animal"
                },
                new Product
                {
                    Id = "956192cb-d2a2-4957-8c4a-3500a536019c",
                    Name = "LEGO ASTRONOMY IN FLY",
                    Price = 135_000,
                    Quantity = 200,
                    Description = "Nowadays, instead of playing with iPads, cell phones, watching TV, etc., which increases the possibility of screen light harming your eyes, playing with educational toys will help you both relax and Develop thinking ability and creativity!",
                    ImagePath = "https://localhost:7001/Images/AstronomyLego.png",
                    ImageLocalPath = @"Images\AstronomyLego.png",
                    CategoryName = "Lego"
                },
                new Product
                {
                    Id = "65c37ebd-95ab-429b-844c-1e230d1a14ec",
                    Name = "SHIBA HIPHOP FLEXIBLE KEYCHAIN",
                    Price = 35_000,
                    Quantity = 300,
                    Description = "Keychains are a more convenient key decoration and hanging product, helping you show off the difference in your key cluster. Using a key hanger will bring you more convenience and ease when storing and carrying with you, limiting misplacement or loss.",
                    ImagePath = "https://localhost:7001/Images/ShibaHiphop.png",
                    ImageLocalPath = @"Images\ShibaHipHop.png",
                    CategoryName = "Keychain"
                }
            };

            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}
