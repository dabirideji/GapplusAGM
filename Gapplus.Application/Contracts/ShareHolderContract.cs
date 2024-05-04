using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Gapplus.Application.DTO.ShareHolder.Request;
using Gapplus.Application.DTO.ShareHolder.Response;
using Gapplus.Application.Interfaces;
using Gapplus.Application.Interfaces.IContracts;
using Gapplus.Domain.Models.Base;
using BarcodeGenerator.Models;

namespace Gapplus.Application.Contracts
{
    public class ShareHolderContract : IShareHolderContract
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UsersContext _context;
        public ShareHolderContract(IUnitOfWork unitOfWork, IMapper mapper, UsersContext context)
        {
            _mapper = mapper;
            _context = context;
            _unitOfWork = unitOfWork;

        }

        public async Task<ReadShareHolderDto> CreateShareHolder(CreateShareHolderDto dto)
        {
            try
            {
                var shareHolder = _mapper.Map<ShareHolder>(dto);
                shareHolder.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                await _unitOfWork.ShareHolders.Add(shareHolder);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<ReadShareHolderDto>(shareHolder);
            }
            catch (System.Exception ex)
            {
                // TODO
                throw;
            }
        }

        public async Task<ReadShareHolderDto> UpdateShareHolder(Guid ShareHolderId, UpdateShareHolderDto dto)
        {
            try
            {
                var shareHolder = await _unitOfWork.ShareHolders.Update(ShareHolderId, _mapper.Map<ShareHolder>(dto));
                await _unitOfWork.SaveAsync();
                return _mapper.Map<ReadShareHolderDto>(shareHolder);
            }
            catch (System.Exception ex)
            {
                // TODO
                throw;
            }
        }
        public async Task<bool> RegisterShareHolderToCompany(RegisterShareHolderToCompanyDto dto)
        {
            try
            {
                var shareHolder = await _unitOfWork.ShareHolders.GetById(dto.ShareHolderId);
                var company = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyRegNo == dto.CompanyRegCode);
                if (shareHolder == null)
                {
                    throw new Exception("INVALID SHAREHOLDER ID || SHAREHOLDER NOT FOUND");
                }
                if (company == null)
                {
                    throw new Exception("INVALID COMPANY REG CODE || COMPANY NOT FOUND");
                }
                var check=await  _context.ShareHolderCompanies.AnyAsync(x=>x.CompanyId==company.CompanyId&&x.ShareHolderId==dto.ShareHolderId);
                if(check){
                    throw new Exception("CANNPT PERFORM ACTION || SHAREHOLDER ALREADY REGISTERED TO COMPANY");
                }


                ShareHolderCompanyRelationShip relationship = new()
                {
                    CompanyId = company.CompanyId,
                    Company = company,
                    ShareHolderId = shareHolder.ShareHolderId,
                    Holdings = dto.Holdings
                };

                await _context.ShareHolderCompanies.AddAsync(relationship);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (System.Exception ex)
            {
                // TODO
                throw;
            }
        }
        public async Task<ReadShareHolderDto> ShareHolderLogin(ShareHolderLoginDto dto)
        {
            try
            {
                var allAccounts = await _context.ShareHolders.Where(x => x.emailAddress == dto.emailAddress).ToListAsync();
                ShareHolder account = null;
                allAccounts.ForEach(x =>
                {
                    var check = BCrypt.Net.BCrypt.Verify(dto.Password, x.Password);
                    if (check)
                    {
                        account = x;
                    }
                });
                if (account == null)
                {
                    return null;
                }
                return _mapper.Map<ReadShareHolderDto>(account);
            }
            catch (System.Exception ex)
            {
                // TODO
                throw;
            }
        }

        public async Task<bool> DeleteShareHolder(Guid Id)
        {
            try
            {
                var shareHolder = await _unitOfWork.ShareHolders.GetById(Id);
                if (shareHolder == null)
                {
                    throw new NullReferenceException("UNABLE TO LOCATE SHAREhOLDER || INVALID SHAREHOLDER ID");
                    // return false;
                }
                await _unitOfWork.ShareHolders.Delete(shareHolder);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (System.Exception ex)
            {
                // TODO
                throw;
            }
        }

        public async Task<IEnumerable<ReadShareHolderDto>> GetAllShareHolders()
        {
            try
            {
                return _mapper.Map<List<ReadShareHolderDto>>(await _unitOfWork.ShareHolders.GetAll());
            }
            catch (System.Exception ex)
            {
                // TODO
                throw;
            }
        }

        public async Task<ReadShareHolderDto> GetShareHolder(Guid ShareHolderId)
        {
            try
            {
                var allShareHolders = await _unitOfWork.ShareHolders.GetAll();
                var shareHolder = allShareHolders.Where(x => x.ShareHolderId == ShareHolderId).FirstOrDefault();
                return _mapper.Map<ReadShareHolderDto>(shareHolder);
            }
            catch (System.Exception ex)
            {
                // TODO
                throw;
            }
        }

        public async Task<bool> RegisterForMeeting(Guid ShareHolderId, Guid MeetingId)
        {
            try
            {
                var shareHolder = await _unitOfWork.ShareHolders.GetById(ShareHolderId);
                // var meeting=await _unitOfWork.Meetings.GetById(MeetingId);
                var meeting = await _context.Meetings.Where(x => x.MeetingId == MeetingId).Include(x => x.MeetingDetails).FirstOrDefaultAsync();
                if (meeting == null || shareHolder == null)
                {
                    throw new Exception("INVALID SHAREHOLDER || MEETING ID");
                }
                MeetingRegistration reg = new();
                reg.MeetingId = MeetingId;
                reg.ShareHolderId = ShareHolderId;
                reg.MeetingNumber=12344444;
                // reg.MeetingNumber = meeting.MeetingDetails.id;
                await _unitOfWork.MeetingRegistrations.Add(reg);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (System.Exception ex)
            {
                // TODO
                throw;
            }
        }
    }
}