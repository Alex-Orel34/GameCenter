using CartService.IClients;
using CartService.IRepositories;
using CartService.Models;
using GameCenter.DbModels;
using GameCenter.IRepositories;
using Microsoft.Extensions.Logging;

namespace CartService.Services
{
    public class CartItemService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserCartRepository _userCartRepository;
        private readonly IProductServiceClient _productServiceClient;
        private readonly ILogger<CartItemService> _logger;

        public CartItemService(
            ICartRepository cartRepository,
            IUserCartRepository userCartRepository,
            IProductServiceClient productServiceClient,
            ILogger<CartItemService> logger)
        {
            _cartRepository = cartRepository;
            _userCartRepository = userCartRepository;
            _productServiceClient = productServiceClient;
            _logger = logger;
        }

        public async Task<CartItemDbModel> AddProductToCartAsync(int cartId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

            var product = await _productServiceClient.GetProductByIdAsync(productId);
            
            if (product == null)
                throw new InvalidOperationException($"Product with id {productId} not found");
            
            if (!product.IsAvailable)
                throw new InvalidOperationException($"Product {productId} is not available");
            var cart = await _userCartRepository.GetUserCartByIdAsync(cartId);

            var existingItem = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
            
            if (existingItem != null)
            {
                return await UpdateExistingItemAsync(existingItem, product, quantity);
            }
            else
            {
                return await CreateNewItemAsync(cartId, product, quantity);
            }
        }

        public async Task<CartItemDbModel> UpdateProductQuantityAsync(int cartId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

            var cart = await _userCartRepository.GetUserCartByIdAsync(cartId);
            
            var existingItem = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingItem == null)
                throw new InvalidOperationException($"Product {productId} not found in cart {cartId}");

            var product = await _productServiceClient.GetProductByIdAsync(productId);
            if (product == null)
                throw new InvalidOperationException($"Product with id {productId} not found");

            var totalPrice = product.Price * quantity;

            existingItem.ItemPrice = product.Price;
            existingItem.ItemName = product.Name;
            existingItem.Quantity = quantity;
            existingItem.TotalPrice = totalPrice;
            existingItem.UpdatedAt = DateTime.UtcNow;

            await _cartRepository.UpdateItemByIdAsync(existingItem.Id, existingItem);
            
            return existingItem;
        }

        public async Task<bool> RemoveProductFromCartAsync(int cartId, int productId)
        {
            var cart = await _userCartRepository.GetUserCartByIdAsync(cartId);
            
            var cartItem = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
                throw new InvalidOperationException($"Product {productId} not found in cart {cartId}");

            return await _cartRepository.DeleteItemByIdAsync(cartItem.Id, cartItem);
        }
        public async Task ClearCartAsync(int cartId)
        {
            var cart = await _userCartRepository.GetUserCartByIdAsync(cartId);
            
            if (cart.CartItems != null)
            {
                foreach (var item in cart.CartItems)
                {
                    await _cartRepository.DeleteItemByIdAsync(item.Id, item);
                }
            }
        }

        private async Task<CartItemDbModel> UpdateExistingItemAsync(
            CartItemDbModel existingItem, 
            ProductModel product, 
            int additionalQuantity)
        {
            var newQuantity = existingItem.Quantity + additionalQuantity;
            var newTotalPrice = product.Price * newQuantity;
            
            existingItem.Quantity = newQuantity;
            existingItem.ItemPrice = product.Price;
            existingItem.ItemName = product.Name;
            existingItem.TotalPrice = newTotalPrice;
            existingItem.UpdatedAt = DateTime.UtcNow;
            
            await _cartRepository.UpdateItemByIdAsync(existingItem.Id, existingItem);
            
            return existingItem;
        }

        private async Task<CartItemDbModel> CreateNewItemAsync(
            int cartId, 
            ProductModel product, 
            int quantity)
        {
            var totalPrice = product.Price * quantity;
            
            var cartItem = new CartItemDbModel
            {
                ProductId = product.Id,
                ItemName = product.Name,
                ItemPrice = product.Price,
                Quantity = quantity,
                TotalPrice = totalPrice,
                UserCartId = cartId,
                CreatedAt = DateTime.UtcNow
            };
            
            return await _cartRepository.AddItemAsync(cartItem);
        }
    }
}
