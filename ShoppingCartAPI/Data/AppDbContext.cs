using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        public DbSet<CartHeader> cartHeaders { get; set; }
        public DbSet<CartDetails> cartDetails { get; set; }
        
    }
}
