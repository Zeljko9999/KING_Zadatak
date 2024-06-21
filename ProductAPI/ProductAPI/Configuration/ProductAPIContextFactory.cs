using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProductAPI.Configuration
{
    public class ProductAPIContextFactory : IDesignTimeDbContextFactory<ProductAPIContext>
    {
        public ProductAPIContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ProductAPIContext>();
            var connectionString = configuration.GetConnectionString("ConnectionString");
            optionsBuilder.UseSqlServer(connectionString);

            return new ProductAPIContext(optionsBuilder.Options);
        }
    }
}
