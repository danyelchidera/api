using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Repository;
using Service.Contracts;
using Shared.DTOs;

namespace Service
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IRepositoryManager _repo;
        private readonly ILoggerManager _log;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repo, ILoggerManager log, IMapper mapper)
        {
            _repo = repo;
            _log = log;
            _mapper = mapper;
           
        }

        public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
        {
            var company = _repo.Company.GetCompany(companyId,false);
            if (company == null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

            _repo.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            _repo.Save();

            var returnEmployee = _mapper.Map<EmployeeDto>(employeeEntity);

            return returnEmployee;

        }

        public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repo.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
                throw new CompanyNotFoundException(companyId);

            var employeeForCompany = _repo.Employee.GetEmployee(companyId, id, trackChanges: false);
            if (employeeForCompany == null)
                throw new EmployeeNotFoundException(id);
            _repo.Employee.DeleteEmployee(employeeForCompany);
            _repo.Save();
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repo.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeDb = _repo.Employee.GetEmployee(companyId, id, trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);

            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        }

        public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
        {
            var company = _repo.Company.GetCompany(companyId, trackChanges);
            if (company == null)
                throw new CompanyNotFoundException(companyId);

            var employeesFromDb = _repo.Employee.GetEmployees(companyId, trackChanges);

            var employeesDTO = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

            return employeesDTO;
        }
    }
}