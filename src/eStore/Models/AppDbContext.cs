using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace eStore.Models
{

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Brand>().ForSqlServerToTable("Brands")
                .Property(b => b.Id).UseSqlServerIdentityColumn();
            builder.Entity<Product>().ForSqlServerToTable("Products")
                .Property(p => p.Id);
            builder.Entity<Cart>().ForSqlServerToTable("Carts")
                .Property(c => c.Id).UseSqlServerIdentityColumn();
            builder.Entity<CartItem>().ForSqlServerToTable("CartItems")
                .Property(ci => ci.Id).UseSqlServerIdentityColumn();
            builder.Entity<Store>().ForSqlServerToTable("Stores")
                .Property(s => s.Id).UseSqlServerIdentityColumn();
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
    }
}
