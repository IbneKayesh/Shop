using Microsoft.EntityFrameworkCore;

namespace Shop.ERP.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }

        public DbSet<UNITS> UNITS { get; set; }
        public DbSet<PRODUCT_CATEGORY> PRODUCT_CATEGORY { get; set; }
        public DbSet<PRODUCTS> PRODUCTS { get; set; }

        public List<PRODUCT_CATEGORY> ExecuteComplexStoredProcedureAsync()
        {
            var result = Set<PRODUCT_CATEGORY>().FromSqlRaw("EXEC sp_get_product_category").ToList();
            return result;
        }
    }
}
