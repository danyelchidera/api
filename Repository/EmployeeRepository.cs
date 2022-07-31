using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
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

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);

        public Employee GetEmployee(Guid companyId, Guid id, bool trackChanges) =>
            FindByCondition(x => x.CompanyId.Equals(companyId) && x.Id.Equals(id), trackChanges).SingleOrDefault();

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
             var employees =  await FindByCondition(x => x.CompanyId == companyId, trackChanges)
                .OrderBy(x => x.Name)
                .ToListAsync();
            return PagedList<Employee>.ToPagedList(employees, employeeParameters.PageNumber, 
                employeeParameters.PageSize);

        }
    }
}
