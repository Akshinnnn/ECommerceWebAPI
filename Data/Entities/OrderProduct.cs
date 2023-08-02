namespace Data.Entities
{
    public class OrderProduct
    {
        public decimal ProductPrice { get; set; }
        public DateTime? RefundDate { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
