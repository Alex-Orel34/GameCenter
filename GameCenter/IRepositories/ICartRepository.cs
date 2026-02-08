using GameCenter.DbModels;

namespace GameCenter.IRepositories
{
    public interface ICartRepository
    {
        Task<CartItemDbModel> GetItemByIdAsync(int id);
        Task<CartItemDbModel> GetItemsFromCartByCartIdAsync(int id);
        Task<CartItemDbModel> UpdateItemByIdAsync(int id, CartItemDbModel item);
        Task<CartItemDbModel> AddItemAsync(CartItemDbModel item);
        Task<bool> DeleteItemByIdAsync(int id, CartItemDbModel item);
    }
}
