namespace Data.Entities
{
    public class Basket : BaseEntity
    {
        public int ProductQuantity { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
