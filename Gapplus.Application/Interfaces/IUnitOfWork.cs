namespace Gapplus.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserService Users { get; }
        public ICompanyService Companies { get; }

        Task<int> SaveAsync();
        int Save();
    }
}
