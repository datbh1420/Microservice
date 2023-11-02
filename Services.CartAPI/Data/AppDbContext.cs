using BackEnd.CartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.CartAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CartHeader> cartHeaders { get; set; }
        public DbSet<CartDetails> cartDetails { get; set; }

    }
}
