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
        IEnumerable<CompanyDTO> GetAllCompanies(bool trackChanges);
        CompanyDTO GetCompany(Guid companyId, bool trackChanges);
        CompanyDTO CreateCompany(CompanyForCreationDto company);
        IEnumerable<CompanyDTO> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
        (IEnumerable<CompanyDTO> companies, string ids) CreateCompanyCollection
        (IEnumerable<CompanyForCreationDto> companyCollection);

    }
}
