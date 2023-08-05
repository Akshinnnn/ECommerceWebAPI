using Logic.Models.DTO.ProductDTO;

namespace Logic.Models.DTO.ProductionCompanyDTO
{
    public class GetCompanyDTO
    {
        public string CompanyName { get; set; }
        public ICollection<GetProductDTO> Products { get; set; }
    }
}
