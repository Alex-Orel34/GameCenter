using CartService.IClients;
using CartService.Models;
using CartService.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace CartService.Clients
{
    public class ProductServiceClient : IProductServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductServiceClient> _logger;
        private readonly ProductServiceOptions _options;

        public ProductServiceClient(
            HttpClient httpClient,
            IOptions<ProductServiceOptions> options,
            ILogger<ProductServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _options = options.Value;
            
            if (_httpClient.BaseAddress == null && !string.IsNullOrEmpty(_options.BaseUrl))
            {
                _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            }
        }

        public async Task<ProductModel?> GetProductByIdAsync(int productId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/products/{productId}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Product with id {ProductId} not found", productId);
                    return null;
                }

                response.EnsureSuccessStatusCode();
                
                var product = await response.Content.ReadFromJsonAsync<ProductModel>();
                return product;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error getting product {ProductId} from ProductService", productId);
                throw new InvalidOperationException($"Failed to get product {productId} from ProductService", ex);
            }
        }

        public async Task<Dictionary<int, ProductModel>> GetProductsByIdsAsync(IEnumerable<int> productIds)
        {
            var productIdsList = productIds.ToList();
            if (!productIdsList.Any())
                return new Dictionary<int, ProductModel>();

            try
            {
                var requestBody = JsonSerializer.Serialize(productIdsList);
                var content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("/api/products/batch", content);
                response.EnsureSuccessStatusCode();
                
                var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>() 
                    ?? new List<ProductModel>();
                
                return products.ToDictionary(p => p.Id, p => p);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error getting products from ProductService");
                throw new InvalidOperationException("Failed to get products from ProductService", ex);
            }
        }
    }
}
