namespace Data.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }
        public Guid OrderNumber { get; set; }
        public int ProductQuantity { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
