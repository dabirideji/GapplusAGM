using AutoMapper;
using Gapplus.Application.DTO.Company.Request;
using Gapplus.Application.DTO.Company.Response;
using Gapplus.Application.Interfaces;
using Gapplus.Application.Interfaces.Contracts;
using Gapplus.Domain;
using Microsoft.Extensions.Logging;

namespace Gapplus.Application.Services
{
    public class CompanyContract : ICompanyContract
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICompanyService _unit;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyContract> _logger;

        public CompanyContract(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CompanyContract> logger
        )
        {
            _unitOfWork = unitOfWork;
            _unit = _unitOfWork.Companies;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReadCompanyDto> CreateCompany(CreateCompanyDto dto)
        {
            var company = _mapper.Map<Company>(dto);

            var createdCompany = await _unit.Add(company);
            if (createdCompany == null)
            {
                throw new Exception("UNABLE TO CREATE COMPANY");
            }
            await _unitOfWork.SaveAsync();
            var companyDto = _mapper.Map<ReadCompanyDto>(createdCompany);
            return companyDto;
        }

        public async Task<bool> DeleteCompany(Guid CompanyId)
        {
             var deleteSuccessful=await _unit.DeleteById(CompanyId);
             if(deleteSuccessful){
                await _unitOfWork.SaveAsync();
                return true;
             }
             return false;
             
        }

        public async Task<IEnumerable<ReadCompanyDto>> GetAllCompany()
        {
            try
            {
                var companies = await _unit.GetAll();
                var companiesDto = _mapper.Map<List<ReadCompanyDto>>(companies);
                return companiesDto;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ReadCompanyDto>> GetCompanyByField(string Field)
        {
            var companies = await _unit.GetAll();
            var filteredData = companies
                .Where(
                    x =>
                        x.CompanyAddress.ToLower().Contains(Field)
                        || x.CompanyName.ToLower().Contains(Field)
                        || x.CompanyStatus.ToString().ToLower().Contains(Field)
                        || x.CompanyId.ToString().ToLower().Contains(Field)
                )
                .ToList();
            var filteredDataDto = _mapper.Map<List<ReadCompanyDto>>(filteredData);
            return filteredDataDto;
        }

        public async Task<ReadCompanyDto> GetCompanyById(Guid CompanyId)
        {
            var company = await _unit.GetById(CompanyId);
            var companyDto = _mapper.Map<ReadCompanyDto>(company);
            return companyDto;
        }

        public async Task<ReadCompanyDto> UpdateCompany(Guid CompanyId, UpdateCompanyDto dto)
        {
            try
            {
                var company = await _unit.GetById(CompanyId);
                if (company == null)
                {
                    throw new NullReferenceException(
                        "UNABLE TO UPDATE COMPANY  || INVALID COMPANY ID"
                    );
                }
                var updateCompanyRequestDto = _mapper.Map<Company>(dto);
                var updatedCompany = await _unit.Update(CompanyId, updateCompanyRequestDto);
                var updatedCompanyDto = _mapper.Map<ReadCompanyDto>(updatedCompany);
                await _unitOfWork.SaveAsync();
                return updatedCompanyDto;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        ///NOT YET IMPLEMENTED
        public async Task<ReadCompanyDto> DisableCompanyAccount(
            Guid CompanyId,
            UpdateCompanyDto dto
        )
        {
            throw new NotImplementedException();
        }

        public async Task<ReadCompanyDto> EnableCompanyAccount(Guid CompanyId, UpdateCompanyDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
