namespace GameCenter.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public List<CartItemModel> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
        public int TotalItems => Items.Sum(item => item.Quantity);
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
