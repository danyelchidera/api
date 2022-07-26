using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository: RepositoryBase<Employee>, IEmployeeRepository
    {
        private readonly RepositoryContext _context;
        public EmployeeRepository(RepositoryContext context)
            : base(context)
        {
            _context = context;
        }

        public Employee GetEmployee(Guid companyId, Guid id, bool trackChanges) =>
            FindByCondition(x => x.CompanyId.Equals(companyId) && x.Id.Equals(id), trackChanges).SingleOrDefault();
            

        public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges)
        {
            return FindByCondition(x => x.CompanyId == companyId, trackChanges)
                    .OrderBy(x => x.Name).ToList();
        }
    }
}
