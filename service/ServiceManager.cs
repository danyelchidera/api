using Contracts;
using Service.Contracts;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IEmployeeService> _employeeServie;
        private readonly Lazy<ICompanyService> _companyService;
        public ServiceManager(IRepositoryManager repo, ILoggerManager log)
        {
            _employeeServie = new Lazy<IEmployeeService>(() => new EmployeeService(repo, log));
            _companyService = new Lazy<ICompanyService>(() => new CompanyService(repo, log));
        }

        public ICompanyService CompanyService => _companyService.Value;

        public IEmployeeService EmployeeService => _employeeServie.Value;
    }
}
