using Contracts;
using Repository;
using Service.Contracts;

namespace Service
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IRepositoryManager _repo;
        private readonly ILoggerManager _log;

        public EmployeeService(IRepositoryManager repo, ILoggerManager log)
        {
            _repo = repo;
            _log = log;
           
        }
    }
}