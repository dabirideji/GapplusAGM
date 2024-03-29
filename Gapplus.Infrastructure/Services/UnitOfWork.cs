using BarcodeGenerator.Models;
using Gapplus.Application.Interfaces;

namespace Gapplus.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UsersContext _context;
        public UnitOfWork(UsersContext context)
        {
            _context = context;
            Users=new UserService(_context);
            Companies=new Gapplus.Infrastructure.Services.CompanyService(_context);
            
        }
        public IUserService Users {get;private set;}

        public ICompanyService Companies {get;private set;}


        public void Dispose()
        {
            _context.Dispose();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
