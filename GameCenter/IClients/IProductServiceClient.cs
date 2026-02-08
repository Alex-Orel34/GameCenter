using CartService.Models;

namespace CartService.IClients
{
    public interface IProductServiceClient
    {
        /// <summary>
        /// Получить информацию о продукте по ID
        /// </summary>
        Task<ProductModel?> GetProductByIdAsync(int productId);

        /// <summary>
        /// Получить информацию о нескольких продуктах по ID
        /// </summary>
        Task<Dictionary<int, ProductModel>> GetProductsByIdsAsync(IEnumerable<int> productIds);
    }
}
