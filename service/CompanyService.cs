
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class CompanyService: ICompanyService
    {
        private readonly IRepositoryManager _repo;
        private readonly ILoggerManager _log;
        private readonly IMapper _mapper;

        public CompanyService (IRepositoryManager repo, ILoggerManager log, IMapper mapper)
        {
            _repo = repo;
            _log = log;
            _mapper = mapper;
        }

        public CompanyDTO CreateCompany(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);

            _repo.Company.CreateCompany(companyEntity);
            _repo.Save();

            var companyToReturn = _mapper.Map<CompanyDTO>(companyEntity);

            return companyToReturn;
        }

        public (IEnumerable<CompanyDTO> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);

            foreach(var companyEntity in companyEntities)
            {
                _repo.Company.CreateCompany(companyEntity);
            }
            _repo.Save();

            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDTO>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(x => x.Id));

            return (companies: companyCollectionToReturn, ids: ids);
        }

        public IEnumerable<CompanyDTO> GetAllCompanies(bool trackChanges)
        {
                var companies = _repo.Company.GetCompanies(trackChanges);
                var companiesDTO = _mapper.Map<IEnumerable<CompanyDTO>>(companies);

                return companiesDTO;
        }

        public IEnumerable<CompanyDTO> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var companies = _repo.Company.GetByIds(ids, trackChanges);

            if (ids.Count() != companies.Count())
                throw new CollectionByIdsBadRequestException();

            var companiesDTOs = _mapper.Map<IEnumerable<CompanyDTO>>(companies);

            return companiesDTOs;
        }

        public CompanyDTO GetCompany(Guid companyId, bool trackChanges)
        {
           var company = _repo.Company.GetCompany(companyId, trackChanges);
            if(company is null)
            {
                throw new CompanyNotFoundException(companyId);
            }
            var companyDTO = _mapper.Map<CompanyDTO>(company);
            return companyDTO;
        }
    }
}
