using Data.Entities.PropertyInterfaces;

namespace Data.Entities
{
    public class Category : BaseEntity, IDateProperties, ISoftDelete
    {
        public Category()
        {
            SubCategories = new HashSet<SubCategory>();    
        }
        public string CategoryName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
