using Microsoft.EntityFrameworkCore;
using WebStore.Data.Entitties;

namespace WebStore.Data
{
    public class AppEFContext : DbContext
    {
        public AppEFContext(DbContextOptions<AppEFContext> options) 
            : base(options) { }
        public DbSet<CategoryEntity> Categories { get; set; }
    }
}
