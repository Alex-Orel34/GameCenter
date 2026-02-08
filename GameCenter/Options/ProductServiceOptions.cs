namespace CartService.Options
{
    /// <summary>
    /// Настройки для клиента ProductService
    /// </summary>
    public class ProductServiceOptions
    {
        public const string SectionName = "ProductService";
        
        public string BaseUrl { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
    }
}
