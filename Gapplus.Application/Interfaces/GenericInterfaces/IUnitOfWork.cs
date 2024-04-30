using Gapplus.Application.Interfaces.GenericInterfaces;

namespace Gapplus.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserService Users { get; }
        public ICompanyService Companies { get; }
        public IMeetingService Meetings {get;}
        public IMeetingRegistrationService MeetingRegistrations {get;}
        public IShareHolderService ShareHolders {get;}

        Task<int> SaveAsync();
        int Save();
    }
}
