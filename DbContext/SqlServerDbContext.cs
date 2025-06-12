using Microsoft.EntityFrameworkCore;
using My_Pos.Models;

namespace My_Pos.DbContexts
{
    public class SqlServerDbContext : DbContext
    {
        public DbSet<Products> Products { get; set; }
        public DbSet<Category> Category { get; set; }
        
        public DbSet<SizeModel> Sizes { get; set; }

        public DbSet<CrustModel> Crust { get; set; }
     


        // Add constructor for dependency injection
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
            : base(options)
        {
        }

        // Remove the OnConfiguring method if using DI configuration
        // Keep only if you need to add additional configuration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Only include if you need to add extra configuration
            // Call base first if overriding
            base.OnConfiguring(optionsBuilder);

            // Optional: Add debug logging or other configurations
            // optionsBuilder.LogTo(Console.WriteLine);
        }
    }
}