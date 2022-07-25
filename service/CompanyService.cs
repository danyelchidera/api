
using Contracts;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CompanyService: ICompanyService
    {
        private readonly IRepositoryManager _repo;
        private readonly ILoggerManager _log;

        public CompanyService (IRepositoryManager repo, ILoggerManager log)
        {
            _repo = repo;
            _log = log;
        }
        
    }
}
