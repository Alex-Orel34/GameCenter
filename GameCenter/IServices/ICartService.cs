using GameCenter.Models;

namespace CartService.IServices
{
    public interface ICartService
    {
        /// <summary>
        /// Получить корзину
        /// </summary>
        Task<CartModel> GetCartAsync(int cartId);

        /// <summary>
        /// Добавить продукт в корзину
        /// </summary>
        Task<CartModel> AddProductToCartAsync(int cartId, int productId, int quantity);

        /// <summary>
        /// Удалить продукт из корзины
        /// </summary>
        Task<CartModel> RemoveProductFromCartAsync(int cartId, int productId);

        /// <summary>
        /// Обновить количество продукта в корзине
        /// </summary>
        Task<CartModel> UpdateProductQuantityAsync(int cartId, int productId, int quantity);

        /// <summary>
        /// Очистить корзину
        /// </summary>
        Task<CartModel> ClearCartAsync(int cartId);
    }
}
