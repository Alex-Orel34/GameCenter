using CartService.IClients;
using CartService.Models;
using GameCenter.DbModels;
using GameCenter.Models;

namespace CartService.Services
{
    public class CartMappingService
    {
        public CartModel MapToCartModel(
            UserCartDbModel cart, 
            Dictionary<int, ProductModel> products)
        {
            var cartItems = cart.CartItems?.Select(ci => MapToCartItemModel(ci, products)).ToList() 
                ?? new List<CartItemModel>();

            return new CartModel
            {
                Id = cart.Id,
                Items = cartItems,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt
            };
        }

        private CartItemModel MapToCartItemModel(
            CartItemDbModel cartItem, 
            Dictionary<int, ProductModel> products)
        {
            var product = products.GetValueOrDefault(cartItem.ProductId);
            
            return new CartItemModel
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product?.Name ?? cartItem.ItemName,
                ProductPrice = cartItem.ItemPrice,
                Quantity = cartItem.Quantity,
                AddedAt = cartItem.CreatedAt
            };
        }
    }
}
