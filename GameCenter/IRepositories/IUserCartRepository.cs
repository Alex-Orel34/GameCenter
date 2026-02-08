using GameCenter.DbModels;

namespace CartService.IRepositories
{
    public interface IUserCartRepository
    {
        Task<UserCartDbModel> GetUserCartByIdAsync(int id);
        Task<UserCartDbModel> GetItemFromCartAsync(int cartId, int itemId);
        Task<UserCartDbModel> DeleteUserCartAsync(int cardId, int userId);
        Task<UserCartDbModel> CreateUserCartAsync(UserCartDbModel cart);
        Task<UserCartDbModel> UpdateUserCartAsync(int id, CartItemDbModel item);
    }
}
