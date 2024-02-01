using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AccessoriesWebsite.Models
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration configuration;

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options , IConfiguration _configuration) :base(options)
        {
            configuration = _configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("connection"));
        }

        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartItem> cartItems { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        public DbSet<ProductOrderItem> productOrderItems  { get; set; }
        public DbSet<ProductCartItem> productCartItems{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configure Identity-related entities
            builder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            builder.Entity<IdentityUserClaim<string>>().HasKey(c => c.Id);
            builder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            builder.Entity<IdentityRoleClaim<string>>().HasKey(rc => rc.Id);

            /////
            ///

            builder.Entity<ApplicationUser>().HasMany(u => u.orders).WithOne(o => o.user).HasForeignKey(o => o.userId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ApplicationUser>().HasOne(u => u.cart).WithOne(c => c.user).HasForeignKey<Cart>(c=>c.userId).OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Order>().HasMany(u => u.orderItems).WithOne(o => o.order).HasForeignKey(o => o.orderId);


            builder.Entity<Category>().HasMany(u => u.products).WithOne(o => o.category).HasForeignKey(o => o.categoryId).OnDelete(DeleteBehavior.NoAction);


            builder.Entity<ProductOrderItem>()
            .HasKey(po => new { po.productId, po.orderItemId });

            builder.Entity<ProductOrderItem>()
                .HasOne(sc => sc.product)
                .WithMany(s => s.productOrderItems)
                .HasForeignKey(sc => sc.productId);

            builder.Entity<ProductOrderItem>()
                .HasOne(sc => sc.orderItem)
                .WithMany(c => c.productOrderItems)
                .HasForeignKey(sc => sc.orderItemId);


            builder.Entity<ProductCartItem>()
            .HasKey(po => new { po.productId, po.cartItemId });

            builder.Entity<ProductCartItem>()
                .HasOne(sc => sc.product)
                .WithMany(s => s.productCartItems)
                .HasForeignKey(sc => sc.productId);

            builder.Entity<ProductCartItem>()
                .HasOne(sc => sc.cartItem)
                .WithMany(c => c.productCartItems)
                .HasForeignKey(sc => sc.cartItemId);


            builder.Entity<Cart>().HasMany(u => u.cartItems).WithOne(o => o.cart).HasForeignKey(o => o.cartId).OnDelete(DeleteBehavior.Cascade); ;


            builder.Entity<Cart>()
            .HasIndex(e => e.userId)
            .IsUnique(false);




        }
    }
}
