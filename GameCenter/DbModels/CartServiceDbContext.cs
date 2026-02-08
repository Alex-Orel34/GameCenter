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
            modelBuilder.HasDefaultSchema("cart");
        }
    }
}
