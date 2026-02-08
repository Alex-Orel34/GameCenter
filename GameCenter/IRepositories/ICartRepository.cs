using GameCenter.DbModels;

namespace GameCenter.IRepositories
{
    public interface ICartRepository
    {
        Task<CartItemDbModel> GetItemByIdAsync(int id);
        Task<CartItemDbModel> GetItemsFromCartByCartIdAsync(int id);
        Task<CartItemDbModel> UpdateItemByIdAsync(int id);
        Task<CartItemDbModel> AddItemAsync(CartItemDbModel item);
        Task<CartItemDbModel> DeleteItemByIdAsync(int id);

    }
}
