using Logic.Models.DTO.ProductionCompanyDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IProductionCompanyService
    {
        Task<bool> Add(AddCompanyDTO companyDTO);
        Task<bool> Update(UpdateCompanyDTO companyDTO);
        Task<bool> SoftDelete(int id);
        Task<IEnumerable<GetCompanyDTO>> GetAll();
        Task<GetCompanyDTO> GetById(int id);    
    }
}
