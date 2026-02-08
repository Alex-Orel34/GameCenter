using CartService.IServices;
using GameCenter.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        /// <summary>
        /// Получить корзину по ID
        /// </summary>
        [HttpGet("{cartId}")]
        [ProducesResponseType(typeof(CartModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CartModel>> GetCart(int cartId)
        {
            _logger.LogInformation("Getting cart. CartId: {CartId}", cartId);
            
            try
            {
                var cart = await _cartService.GetCartAsync(cartId);
                return Ok(cart);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Cart not found. CartId: {CartId}", cartId);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart. CartId: {CartId}", cartId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Добавить продукт в корзину
        /// </summary>
        [HttpPost("{cartId}/items")]
        [ProducesResponseType(typeof(CartModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CartModel>> AddProductToCart(
            int cartId, 
            [FromBody] AddProductToCartRequest request)
        {
            _logger.LogInformation(
                "Adding product to cart. CartId: {CartId}, ProductId: {ProductId}, Quantity: {Quantity}",
                cartId, request.ProductId, request.Quantity);

            try
            {
                var cart = await _cartService.AddProductToCartAsync(cartId, request.ProductId, request.Quantity);
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request. CartId: {CartId}", cartId);
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operation failed. CartId: {CartId}", cartId);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product to cart. CartId: {CartId}", cartId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Удалить продукт из корзины
        /// </summary>
        [HttpDelete("{cartId}/items/{productId}")]
        [ProducesResponseType(typeof(CartModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CartModel>> RemoveProductFromCart(int cartId, int productId)
        {
            _logger.LogInformation(
                "Removing product from cart. CartId: {CartId}, ProductId: {ProductId}",
                cartId, productId);

            try
            {
                var cart = await _cartService.RemoveProductFromCartAsync(cartId, productId);
                return Ok(cart);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Product not found in cart. CartId: {CartId}, ProductId: {ProductId}", 
                    cartId, productId);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing product from cart. CartId: {CartId}, ProductId: {ProductId}",
                    cartId, productId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Обновить количество продукта в корзине
        /// </summary>
        [HttpPut("{cartId}/items/{productId}")]
        [ProducesResponseType(typeof(CartModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CartModel>> UpdateProductQuantity(
            int cartId, 
            int productId, 
            [FromBody] UpdateQuantityRequest request)
        {
            _logger.LogInformation(
                "Updating product quantity. CartId: {CartId}, ProductId: {ProductId}, Quantity: {Quantity}",
                cartId, productId, request.Quantity);

            try
            {
                var cart = await _cartService.UpdateProductQuantityAsync(cartId, productId, request.Quantity);
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid quantity. CartId: {CartId}, ProductId: {ProductId}", 
                    cartId, productId);
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Product not found in cart. CartId: {CartId}, ProductId: {ProductId}", 
                    cartId, productId);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product quantity. CartId: {CartId}, ProductId: {ProductId}",
                    cartId, productId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        [HttpDelete("{cartId}")]
        [ProducesResponseType(typeof(CartModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CartModel>> ClearCart(int cartId)
        {
            _logger.LogInformation("Clearing cart. CartId: {CartId}", cartId);

            try
            {
                var cart = await _cartService.ClearCartAsync(cartId);
                return Ok(cart);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Cart not found. CartId: {CartId}", cartId);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart. CartId: {CartId}", cartId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }

    /// <summary>
    /// Запрос на добавление продукта в корзину
    /// </summary>
    public class AddProductToCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Запрос на обновление количества продукта
    /// </summary>
    public class UpdateQuantityRequest
    {
        public int Quantity { get; set; }
    }
}
