using Microsoft.EntityFrameworkCore;

namespace Shop.ERP.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }

        public DbSet<PRODUCT_CATEGORY> PRODUCT_CATEGORY { get; set; }
    }
}
