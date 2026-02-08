using CartService.IClients;
using CartService.IRepositories;
using CartService.IServices;
using GameCenter.DbModels;
using GameCenter.IRepositories;
using GameCenter.Models;

namespace CartService.Services
{
    public class CartService : ICartService
    {
        private readonly IUserCartRepository _userCartRepository;
        private readonly IProductServiceClient _productServiceClient;
        private readonly CartItemService _cartItemService;
        private readonly CartTotalsService _cartTotalsService;
        private readonly CartMappingService _cartMappingService;

        public CartService(
            IUserCartRepository userCartRepository,
            IProductServiceClient productServiceClient,
            CartItemService cartItemService,
            CartTotalsService cartTotalsService,
            CartMappingService cartMappingService)
        {
            _userCartRepository = userCartRepository;
            _productServiceClient = productServiceClient;
            _cartItemService = cartItemService;
            _cartTotalsService = cartTotalsService;
            _cartMappingService = cartMappingService;
        }

        public async Task<CartModel> GetCartAsync(int cartId)
        {
            var cart = await _userCartRepository.GetUserCartByIdAsync(cartId);
 
            var productIds = cart.CartItems?.Select(ci => ci.ProductId).Distinct().ToList() ?? new List<int>();
            var products = await _productServiceClient.GetProductsByIdsAsync(productIds);
            
            return _cartMappingService.MapToCartModel(cart, products);
        }

        public async Task<CartModel> AddProductToCartAsync(int cartId, int productId, int quantity)
        {
            await _cartItemService.AddProductToCartAsync(cartId, productId, quantity);
            
            await _cartTotalsService.RecalculateCartTotalsAsync(cartId);

            return await GetCartAsync(cartId);
        }

        public async Task<CartModel> RemoveProductFromCartAsync(int cartId, int productId)
        {
            await _cartItemService.RemoveProductFromCartAsync(cartId, productId);
            
            await _cartTotalsService.RecalculateCartTotalsAsync(cartId);

            return await GetCartAsync(cartId);
        }

        public async Task<CartModel> UpdateProductQuantityAsync(int cartId, int productId, int quantity)
        {
            await _cartItemService.UpdateProductQuantityAsync(cartId, productId, quantity);
           
            await _cartTotalsService.RecalculateCartTotalsAsync(cartId);

            return await GetCartAsync(cartId);
        }

        public async Task<CartModel> ClearCartAsync(int cartId)
        {
            await _cartItemService.ClearCartAsync(cartId);
            
            await _cartTotalsService.RecalculateCartTotalsAsync(cartId);

            return await GetCartAsync(cartId);
        }
    }
}
