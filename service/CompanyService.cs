
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

        public IEnumerable<CompanyDTO> GetAllCompanies(bool trackChanges)
        {
                var companies = _repo.Company.GetCompanies(trackChanges);
                var companiesDTO = _mapper.Map<IEnumerable<CompanyDTO>>(companies);

                return companiesDTO;
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
