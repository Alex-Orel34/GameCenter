using CartService.DbModels;
using GameCenter.DbModels;
using GameCenter.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Repositories
{
    public class CartRepository : ICartRepository
    {
        public CartRepository(CartServiceDbContext context)
        {
            _context = context;
        }
        public Task<CartItemDbModel> AddItemAsync(CartItemDbModel item)
        {
            throw new NotImplementedException();
        }

        public Task<CartItemDbModel> DeleteItemByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CartItemDbModel> GetItemByIdAsync(int id) => _context.

        public Task<CartItemDbModel> GetItemsFromCartByCartIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CartItemDbModel> UpdateItemByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
