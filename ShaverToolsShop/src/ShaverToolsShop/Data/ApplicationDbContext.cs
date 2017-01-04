using Microsoft.EntityFrameworkCore;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

    }
}