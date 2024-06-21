using ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Configuration
{
    public class ProductAPIContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null;
        public DbSet<Dimensions> Dimensions { get; set; } = null;
        public DbSet<Meta> Meta { get; set; } = null;
        public DbSet<Review> Reviews { get; set; } = null;


        public ProductAPIContext(DbContextOptions<ProductAPIContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
