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
    }
}
