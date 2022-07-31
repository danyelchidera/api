using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync(bool trackChanges);
        Task<CompanyDTO> GetCompanyAsync(Guid companyId, bool trackChanges);
        Task<CompanyDTO> CreateCompanyAsync(CompanyForCreationDto company);
        Task<IEnumerable<CompanyDTO>> GetByIdsAsync(IEnumerable<Guid> ids, bool
        trackChanges);
        Task<(IEnumerable<CompanyDTO> companies, string ids)>
        CreateCompanyCollectionAsync
        (IEnumerable<CompanyForCreationDto> companyCollection);
        Task DeleteCompanyAsync(Guid companyId, bool trackChanges);
        Task UpdateCompanyAsync(Guid companyid, CompanyForUpdateDto companyForUpdate,
        bool trackChanges);


    }
}
