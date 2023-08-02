using Data.Entities.PropertyInterfaces;

namespace Data.Entities
{
    public class ProductInformation : BaseEntity, IDateProperties, ISoftDelete
    {
        public string Header { get; set; }
        public string Description { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
