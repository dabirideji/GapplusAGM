using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gapplus.Domain;



namespace Gapplus.Application.Interfaces
{
    public interface IUserService : IGenericService<User> {
        Task<User> UserLogin(User entity);
     }
}
