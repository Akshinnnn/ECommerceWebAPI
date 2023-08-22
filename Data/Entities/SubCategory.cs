using Data.Entities.PropertyInterfaces;
using System.Collections;

namespace Data.Entities
{
    public class SubCategory : BaseEntity, IDateProperties, ISoftDelete
    {
        public SubCategory()
        {
            Products = new HashSet<Product>();
        }
        public string SubCategoryName { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
