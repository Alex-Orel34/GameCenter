using CartService.DbModels;
using GameCenter.DbModels;
using GameCenter.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartServiceDbContext _context;

        public CartRepository(CartServiceDbContext context)
        {
            _context = context;
        }
        public async Task<CartItemDbModel> AddItemAsync(CartItemDbModel item)
        {
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteItemByIdAsync(int id, CartItemDbModel item)
        {
            var existingItem = await _context.CartItems.FirstOrDefaultAsync(i => i.Id == id);

            if (existingItem is null)
                return false;

            _context.CartItems.Remove(existingItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CartItemDbModel> GetItemByIdAsync(int id)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(i => i.Id == id);
            if (item is null)
                throw new InvalidOperationException($"Cart item with id {id} not found");
            return item;
        }

        public async Task<CartItemDbModel> GetItemsFromCartByCartIdAsync(int id)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(i => i.UserCartId == id);
            if (item is null)
                throw new InvalidOperationException($"Cart item with UserCartId {id} not found");
            return item;
        }

        public async Task<CartItemDbModel> UpdateItemByIdAsync(int id, CartItemDbModel item)
        {
            var existingItem = await _context.CartItems.FirstOrDefaultAsync(i => i.Id == id);
            if (existingItem is null)
                throw new InvalidOperationException($"Cart item with id {id} not found");

            existingItem.ItemName = item.ItemName;
            existingItem.ItemPrice = item.ItemPrice;
            existingItem.Quantity = item.Quantity;
            existingItem.TotalPrice = item.TotalPrice;
            existingItem.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return existingItem;
        }
    }
}
