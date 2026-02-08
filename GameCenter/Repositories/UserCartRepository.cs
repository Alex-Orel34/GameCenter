using CartService.DbModels;
using CartService.IRepositories;
using GameCenter.DbModels;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Repositories
{
    public class UserCartRepository : IUserCartRepository
    {
        private readonly CartServiceDbContext _context;

        public UserCartRepository(CartServiceDbContext context)
        {
            _context = context;
        }

        public async Task<UserCartDbModel> CreateUserCartAsync(UserCartDbModel cart)
        {
            cart.CreatedAt = DateTime.UtcNow;
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<UserCartDbModel> DeleteUserCartAsync(int cardId, int userId)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.Id == cardId && c.UserId == userId);
            
            if (cart == null)
                throw new InvalidOperationException($"Cart with id {cardId} for user {userId} not found");

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<UserCartDbModel> GetItemFromCartAsync(int cartId, int itemId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == cartId);
            
            if (cart == null)
                throw new InvalidOperationException($"Cart with id {cartId} not found");

            var cartItem = cart.CartItems?.FirstOrDefault(ci => ci.Id == itemId);
            
            if (cartItem == null)
                throw new InvalidOperationException($"Cart item with id {itemId} not found in cart {cartId}");

            return cart;
        }

        public async Task<UserCartDbModel> GetUserCartByIdAsync(int id)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (cart == null)
                throw new InvalidOperationException($"Cart with id {id} not found");
            
            return cart;
        }

        public async Task<UserCartDbModel> UpdateUserCartAsync(int id, CartItemDbModel item)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (cart == null)
                throw new InvalidOperationException($"Cart with id {id} not found");

            var existingItem = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == item.ProductId);
            
            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
                existingItem.TotalPrice = item.TotalPrice;
                existingItem.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                item.UserCartId = id;
                item.CreatedAt = DateTime.UtcNow;
                _context.CartItems.Add(item);
            }
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await _context.Entry(cart).ReloadAsync();
            await _context.Entry(cart).Collection(c => c.CartItems).LoadAsync();
            
            return cart;
        }

        public async Task UpdateCartTotalsAsync(int cartId, int countOfItems, decimal totalCartPrice)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null)
                throw new InvalidOperationException($"Cart with id {cartId} not found");

            cart.CountOfItems = countOfItems;
            cart.TotalCartPrrice = totalCartPrice;
            cart.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
        }
    }
}
