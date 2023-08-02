using Data.Entities.PropertyInterfaces;

namespace Data.Entities
{
    public class ProductionCompany : BaseEntity, IDateProperties, ISoftDelete
    {
        public string CompanyName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
