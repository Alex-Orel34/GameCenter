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

        public Task<UserCartDbModel> CreateUserCartAsync(UserCartDbModel cart)
        {
            throw new NotImplementedException();
        }

        public Task<UserCartDbModel> DeleteUserCartAsync(int cardId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserCartDbModel> GetItemFromCartAsync(int cartId, int itemId)
        {
            throw new NotImplementedException();
        }

        public Task<UserCartDbModel> GetUserCartByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserCartDbModel> UpdateUserCartAsync(int id, CartItemDbModel item)
        {
            throw new NotImplementedException();
        }
    }
}
