using AutoMapper;
using Contracts;
using Service.Contracts;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IEmployeeService> _employeeServie;
        private readonly Lazy<ICompanyService> _companyService;
        public ServiceManager(IRepositoryManager repo, ILoggerManager log, IMapper mapper)
        {
            _employeeServie = new Lazy<IEmployeeService>(() => new EmployeeService(repo, log, mapper));
            _companyService = new Lazy<ICompanyService>(() => new CompanyService(repo, log, mapper));
        }

        public ICompanyService CompanyService => _companyService.Value;

        public IEmployeeService EmployeeService => _employeeServie.Value;
    }
}
