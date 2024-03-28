using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gapplus.Application.Interfaces
{
    public interface IGenericService<T> where T:class
    {
        Task<T> Add(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid Id);
        Task<T> Update(Guid Id,T entity);
        Task<bool> DeleteById(Guid Id);
        Task<bool> Delete(T entity);
    }
}