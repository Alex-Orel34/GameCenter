using GameCenter.DbModels;
using CartService.IRepositories;

namespace CartService.Services
{
    public class CartTotalsService
    {
        private readonly IUserCartRepository _userCartRepository;

        public CartTotalsService(IUserCartRepository userCartRepository)
        {
            _userCartRepository = userCartRepository;
        }

        public async Task RecalculateCartTotalsAsync(int cartId)
        {
            var cart = await _userCartRepository.GetUserCartByIdAsync(cartId);
            
            var countOfItems = cart.CartItems?.Count ?? 0;
            var totalCartPrice = cart.CartItems?.Sum(ci => ci.TotalPrice) ?? 0;
            
            await _userCartRepository.UpdateCartTotalsAsync(cartId, countOfItems, totalCartPrice);
        }
    }
}
