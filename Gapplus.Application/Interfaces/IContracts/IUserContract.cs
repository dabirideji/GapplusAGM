using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gapplus.Application.DTO.Company.Request;
using Gapplus.Application.DTO.Company.Response;
using Gapplus.Application.DTO.User.Request;
using Gapplus.Application.DTO.User.Response;

namespace Gapplus.Application.Interfaces.Contracts
{
    public interface IUserContract
    {
        //CREATE
        Task<ReadUserDto> CreateUser(Guid CompanyId,CreateUserDto dto);

        //READ
        Task<IEnumerable<ReadUserDto>> GetAllUsers();
        Task<ReadUserDto> GetUserById(Guid UserId);
        Task<IEnumerable<ReadUserDto>> GetUserByField(String Field);
        Task<IEnumerable<ReadUserDto>> GetUserByCompanyId(Guid CompanyId);
        Task<ReadUserDto> UserLogin(UserLoginDto dto);


        //UPDATE
        Task<ReadUserDto> UpdateUser(Guid UserId,UpdateUserDto dto);
        Task<ReadUserDto> DisableUserAccount(Guid UserId,UpdateUserDto dto);
        Task<ReadUserDto> EnableUserAccount(Guid UserId,UpdateUserDto dto);

        //DELETE 
        Task<bool> DeleteUser(Guid UserId);


    }
    public interface ICompanyContract
    {
        //CREATE
        Task<ReadCompanyDto> CreateCompany(CreateCompanyDto dto);

        //READ
        Task<IEnumerable<ReadCompanyDto>> GetAllCompany();
        Task<ReadCompanyDto> GetCompanyById(Guid CompanyId);
        Task<IEnumerable<ReadCompanyDto>> GetCompanyByField(String Field);
        Task<IEnumerable<ReadCompanyDto>> GetShareHolderCompanies(Guid ShareHolderId);


        //UPDATE
        Task<ReadCompanyDto> UpdateCompany(Guid CompanyId,UpdateCompanyDto dto);
        Task<ReadCompanyDto> DisableCompanyAccount(Guid CompanyId,UpdateCompanyDto dto);
        Task<ReadCompanyDto> EnableCompanyAccount(Guid CompanyId,UpdateCompanyDto dto);

        //DELETE 
        Task<bool> DeleteCompany(Guid CompanyId);


    }
}