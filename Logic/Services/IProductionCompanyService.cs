using Logic.Models.DTO.ProductionCompanyDTO;
using Logic.Models.GenericResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IProductionCompanyService
    {
        Task<GenericResponse<bool>> Add(AddCompanyDTO companyDTO);
        Task<GenericResponse<bool>> Update(UpdateCompanyDTO companyDTO);
        Task<GenericResponse<bool>> SoftDelete(int id);
        Task<GenericResponse<IEnumerable<GetCompanyDTO>>> GetAll();
        Task<GenericResponse<GetCompanyDTO>> GetById(int id);    
    }
}
