using Data.Entities.PropertyInterfaces;

namespace Data.Entities
{
    public class Product : BaseEntity, IDateProperties, ISoftDelete
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public int DiscountPercent { get; set; }
        public int Quantity { get; set; }

        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public int CompanyId { get; set; }
        public ProductionCompany ProductionCompany { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<ProductImage> Images { get; set; }
        public ICollection<ProductInformation> ProductInformations { get; set; }
        public ICollection<Basket> Baskets { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
