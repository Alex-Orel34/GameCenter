namespace GameCenter.DbModels
{
    public class CartItemDbModel: BaseDbModel
    {
        public int ProductId { get; set; }
        public int UserCartId { get; set; }
        public int UserId { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => ProductPrice * Quantity;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public UserCartDbModel? UserCart { get; set; }
    }
}
