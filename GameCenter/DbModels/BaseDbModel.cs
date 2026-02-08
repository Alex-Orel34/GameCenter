namespace GameCenter.DbModels
{
    public abstract class BaseDbModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
