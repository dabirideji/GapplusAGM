using BarcodeGenerator.Models;
using Gapplus.Application.Interfaces;
using Gapplus.Domain;

namespace Gapplus.Infrastructure.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly UsersContext _context;

        public UserService(UsersContext context) : base(context)
        {
            _context = context;
        }

        public  async Task<User> UserLogin(User entity)
        {
            // var user=await _context.UserProfiles.FirstOrDefaultAsync(x=>x.FullName==entity.FullName||x.EmailId==entity.EmailId);
            // if(user==null){
            //     throw new UnauthorizedAccessException("LOGIN FAILED || INVALID FullName OR USEREMAIL");
            // }
            // var passwordCheck=BCrypt.Net.BCrypt.Verify(entity.UserPassword,user);
            // if(!passwordCheck){
            //     throw new UnauthorizedAccessException("LOGIN FAILED || INVALID PASSWORD");

            // }
            // return user;

            throw new NotImplementedException("METHOD NOT IMPLEMENTED || CURRENTLY UNDERGOING AN UPGRADE");
        }
    }
}
