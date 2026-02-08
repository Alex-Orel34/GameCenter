namespace GameCenter.DbModels
{
    public class CartItemDbModel: BaseDbModel
    {
        public int ProductId { get; set; }
        public int UserCartId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice {get; set; }
        public UserCartDbModel? UserCart { get; set; }
    }
}
