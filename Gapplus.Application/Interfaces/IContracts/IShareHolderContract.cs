using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gapplus.Application.DTO.ShareHolder.Request;
using Gapplus.Application.DTO.ShareHolder.Response;

namespace Gapplus.Application.Interfaces.IContracts
{
    public interface IShareHolderContract
    {
        Task<ReadShareHolderDto> CreateShareHolder(CreateShareHolderDto dto);
        Task<ReadShareHolderDto> ShareHolderLogin(ShareHolderLoginDto dto);
        Task<IEnumerable<ReadShareHolderDto>> GetAllShareHolders();
        Task<ReadShareHolderDto> GetShareHolder(Guid ShareHolderId);
        Task<bool> RegisterForMeeting(Guid ShareHolderId,Guid MeetingId);
        Task<bool> RegisterShareHolderToCompany(RegisterShareHolderToCompanyDto dto);
        Task<ReadShareHolderDto> UpdateShareHolder(Guid ShareHolderId,UpdateShareHolderDto dto);
        Task<bool> DeleteShareHolder(Guid Id);

    }
}