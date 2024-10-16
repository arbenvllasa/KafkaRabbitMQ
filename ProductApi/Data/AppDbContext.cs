using Microsoft.EntityFrameworkCore;
using Shared;

namespace ProductApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options) { }
        public DbSet<Product> Products { get; set; }

    }
}
