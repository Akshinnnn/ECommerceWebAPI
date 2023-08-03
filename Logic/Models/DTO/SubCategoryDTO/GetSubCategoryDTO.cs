using Data.Entities;
using Logic.Models.DTO.ProductDTO;

namespace Logic.Models.DTO.SubCategoryDTO
{
    public class GetSubCategoryDTO
    {
        public string SubCategoryName { get; set; }
        public ICollection<GetProductDTO> Products { get; set; }
    }
}
