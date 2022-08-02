using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Repository;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repo;
        private readonly ILoggerManager _log;
        private readonly IMapper _mapper;
        private readonly IDataShaper<EmployeeDto> _dataShaper;

        public EmployeeService(IRepositoryManager repo, ILoggerManager log, IMapper mapper, IDataShaper<EmployeeDto> dataShaper)
        {
            _repo = repo;
            _log = log;
            _mapper = mapper;
            _dataShaper = dataShaper;
           
        }

        public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

            _repo.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repo.SaveAsync();

            var returnEmployee = _mapper.Map<EmployeeDto>(employeeEntity);

            return returnEmployee;

        }

        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id,
            trackChanges);

            _repo.Employee.DeleteEmployee(employeeDb);

            await _repo.SaveAsync();
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeDb = _repo.Employee.GetEmployee(companyId, id, trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);

            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        }

        public async Task<(IEnumerable<ExpandoObject> employees, MetaData metaData)> 
            GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            if (!employeeParameters.ValidAgeRange)
                throw new MaxAgeRangeBadRequestException();

            await CheckIfCompanyExists(companyId, trackChanges);

            var employeesWithMetaData = await _repo.Employee.GetEmployeesAsync(companyId, 
                employeeParameters, trackChanges);

            var employeesDTO = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
            var shapedData = _dataShaper.ShapeData(employeesDTO, employeeParameters.Fields);

            return (employees: shapedData, metaData: employeesWithMetaData.MetaData);
        }
        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
            Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            await CheckIfCompanyExists(companyId, compTrackChanges);

            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id,
            empTrackChanges);

            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeDb);

            return (employeeToPatch: employeeToPatch, employeeEntity: employeeDb);
        }
        public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repo.SaveAsync();
        }

        public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
        {
            await CheckIfCompanyExists(companyId, compTrackChanges);

            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id,
            empTrackChanges);

            _mapper.Map(employeeForUpdate, employeeDb);

            await _repo.SaveAsync();
        }

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repo.Company.GetCompanyAsync(companyId,
            trackChanges);
            if (company is null)
            throw new CompanyNotFoundException(companyId);
        }
        private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists
        (Guid companyId, Guid id, bool trackChanges)
        {
            var employeeDb = _repo.Employee.GetEmployee(companyId, id,
            trackChanges);

            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);

            return employeeDb;
        }

    }
}