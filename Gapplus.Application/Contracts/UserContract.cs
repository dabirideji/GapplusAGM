using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gapplus.Application.DTO.User.Request;
using Gapplus.Application.DTO.User.Response;
using Gapplus.Application.Interfaces;
using Gapplus.Application.Interfaces.Contracts;
using Gapplus.Domain;
using Microsoft.Extensions.Logging;

namespace Gapplus.Application.Services
{
    public class UserContract : IUserContract
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _unit;
        private readonly IMapper _mapper;
        private readonly ILogger<UserContract> _logger;

        public UserContract(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserContract> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _unit = _unitOfWork.Users;
        }

        public async Task<ReadUserDto> CreateUser(Guid CompanyId, CreateUserDto dto)
        {
            try
            {
                var company = await _unitOfWork.Companies.GetById(CompanyId);
                if (company == null)
                {
                    throw new NullReferenceException("COMPANY NOT FOUND || INVALID COMPANY ID");
                }
                var newUser = _mapper.Map<User>(dto);
                newUser.CompanyInfo = company.CompanyName;

                //HHASHING THE USER PASSWORD FOR ADDED SECCUTIRY
                dto.UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.ConfirmUserPassword);
                var user = await _unit.Add(newUser);
                if (user == null)
                {
                    throw new Exception("COULD NOT ADD USER || SOMETHIBNG WENT WRONG");
                }
                await _unitOfWork.SaveAsync();
                var userDto = _mapper.Map<ReadUserDto>(user);
                userDto.Company = await _unitOfWork.Companies.GetById(userDto.CompanyId);
                return userDto;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUser(Guid UserId)
        {
            try
            {
                var user = await _unit.GetById(UserId);
                if (user == null)
                {
                    throw new NullReferenceException("UNABLE TO DELETE USER || INVALID USER ID");
                }

                var userDeleteRequest = await _unit.Delete(user);
                if (!userDeleteRequest)
                {
                    throw new Exception("UNABLE TO DELETE USER || SOMETHING WENT WRONG");
                }

                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<ReadUserDto> DisableUserAccount(Guid UserId, UpdateUserDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ReadUserDto> EnableUserAccount(Guid UserId, UpdateUserDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReadUserDto>> GetAllUsers()
        {
            var users = await _unit.GetAll();
            var usersDto = _mapper.Map<List<ReadUserDto>>(users);
            return usersDto;
        }

        public async Task<IEnumerable<ReadUserDto>> GetUserByCompanyId(Guid CompanyId)
        {
            try
            {
                var company = await _unitOfWork.Companies.GetById(CompanyId);
                if (company == null)
                {
                    throw new Exception("UNABLE TO LOCATE COMPANY || INVALID COOMPANY ID");
                }
                var allUsers = await _unit.GetAll();

                var companyUsers = allUsers.Where(x => x.CompanyInfo == company.CompanyName).ToList();

                var companyUsersDto = _mapper.Map<List<ReadUserDto>>(companyUsers);
                return companyUsersDto;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ReadUserDto>> GetUserByField(string Field)
        {
            try
            {
                var allUsers = await _unit.GetAll();
                var filteredData = allUsers
                    .Where(
                        x =>
                            x.FirstName.ToLower().Contains(Field)
                            || x.LastName.ToLower().Contains(Field)
                            || x.FullName.ToLower().Contains(Field)
                            || x.EmailId.ToLower().Contains(Field)
                            || x.UserId.ToString().ToLower().Contains(Field)
                            || x.CompanyInfo.ToString().ToLower().Contains(Field)
                    )
                    .ToList();
                var filteredDataDto = _mapper.Map<List<ReadUserDto>>(filteredData);
                // filteredDataDto.Company=await _unitOfWork.Companies.GetById(filteredData.CompanyId);
                return filteredDataDto;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<ReadUserDto> GetUserById(Guid UserId)
        {
            try
            {
                var user = await _unit.GetById(UserId);
                if (user == null)
                {
                    throw new NullReferenceException("UNABLE TO LOCATE USER || INVALID USER ID");
                }
                var userDto = _mapper.Map<ReadUserDto>(user);
                userDto.Company = await _unitOfWork.Companies.GetById(userDto.CompanyId);
                return userDto;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<ReadUserDto> UpdateUser(Guid UserId, UpdateUserDto dto)
        {
            try
            {
                var user = await _unit.GetById(UserId);
                if (user == null)
                {
                    throw new NullReferenceException("UNABLE TO UPDATE USER  || INVALID USER ID");
                }
                var updateUserRequestDto = _mapper.Map<User>(dto);
                var updatedUser = await _unit.Update(UserId, updateUserRequestDto);
                var updatedUserDto = _mapper.Map<ReadUserDto>(updatedUser);
                return updatedUserDto;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<ReadUserDto> UserLogin(UserLoginDto dto)
        {
            try
            {
                var user = new User
                {
                    EmailId = dto.UserNameOrEmail,
                    FullName = dto.UserNameOrEmail,
                    UserPassword = dto.UserPassword,
                    CompanyInfo =new String("")
                };

                var userLoginRequest = await _unit.UserLogin(user);
                var userLoginRequestDto = _mapper.Map<ReadUserDto>(userLoginRequest);
                return userLoginRequestDto;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
