using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    internal sealed class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext context)
            : base(context)
        {
         
        }

        public void CreateCompany(Company company) => Create(company);

        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            return FindByCondition(x => ids.Contains(x.Id), trackChanges)
                .ToList();
        }

        public IEnumerable<Company> GetCompanies(bool trackChanges) => 
            FindAll(trackChanges)
                .OrderBy(x => x.Name)
                .ToList();

        public Company GetCompany(Guid companyId, bool trackChanges)
        {
            var company = FindByCondition(x => x.Id == companyId, trackChanges).SingleOrDefault(); 
            return company;
        }
    }
}
