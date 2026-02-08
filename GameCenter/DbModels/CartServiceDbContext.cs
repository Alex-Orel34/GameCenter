using GameCenter.DbModels;
using GameCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace CartService.DbModels
{
    public class CartServiceDbContext : DbContext
    {
        public CartServiceDbContext(DbContextOptions<CartServiceDbContext> options)
           : base(options)
        {
        }
        public DbSet<CartItemDbModel> CartItems { get; set; }
        public DbSet<UserCartDbModel> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.HasDefaultSchema("cart");

            modelBuilder.Entity<UserCartDbModel>(entity =>
            {
                entity.ToTable("UserCarts", "cart");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.UserId).IsRequired();
                
                entity.HasMany(e => e.CartItems)
                    .WithOne(e => e.UserCart)
                    .HasForeignKey(e => e.UserCartId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItemDbModel>(entity =>
            {
                entity.ToTable("CartItems", "cart");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.UserCartId).IsRequired();
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.ProductPrice).IsRequired()
                    .HasColumnType("decimal(18,2)");
                entity.Property(e => e.AddedAt).IsRequired();

                entity.Ignore(e => e.TotalPrice);
            });
        }
    }
}
