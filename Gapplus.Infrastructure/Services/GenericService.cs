using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarcodeGenerator.Models;
using Gapplus.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gapplus.Infrastructure.Services
{
    public class GenericService<T> : IGenericService<T>
        where T : class
    {
        private readonly UsersContext _context;
        private readonly DbSet<T> _db;
        public GenericService(UsersContext context)
        {
            _context = context;
            _db=_context.Set<T>();            
        }

        public async Task<T> Add(T entity)
        {
            await _db.AddAsync(entity);
            return entity;
        }

        public async Task<bool> Delete(T entity)
        {
           try
           {
             _db.Remove(entity);
             return true;
           }
           catch (System.Exception)
           {
            
            return false;
           }
        }

        public async Task<bool> DeleteById(Guid Id)
        {
            var data=await GetById(Id);
            if(data!=null){
                _db.Remove(data);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _db.ToListAsync();
        }

        public async Task<T> GetById(Guid Id)
        {
            var data= await _db.FindAsync(Id);
            if(data==null){
                throw new NullReferenceException("ENTITY NOT FOUND || INVALID ID");
            }
            return data;
        }

        public async Task<T> Update(Guid Id, T entity)
        {
             var data = await _db.FindAsync(Id);

            if (data == null)
            {
                throw new NullReferenceException("UNABLE TO LOCATE ENTITY || INVALID ID");
            }
            _context.Entry(data).State=EntityState.Modified;
            _context.Entry(data).CurrentValues.SetValues(entity);

            return data;
        }
    }
}
