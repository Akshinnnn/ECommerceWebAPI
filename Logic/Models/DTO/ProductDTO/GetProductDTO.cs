using Data.Entities;
using Logic.Models.DTO.ProductInfoDTO;

namespace Logic.Models.DTO.ProductDTO
{
    public class GetProductDTO
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public ICollection<GetProductInfoDTO> ProductInformations { get; set; }
        public ICollection<ProductImage> Images { get; set; }
    }
}
