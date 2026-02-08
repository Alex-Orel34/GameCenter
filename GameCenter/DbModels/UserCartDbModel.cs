namespace GameCenter.DbModels
{
    public class UserCartDbModel: BaseDbModel
    {
        public int UserId { get; set; }
        public int ItemId {  get; set; }
        public int CountOfItems { get; set; }
        public decimal TotalCartPrrice { get; set; }
        public List<CartItemDbModel> CartItems { get; set; }
    }
}
