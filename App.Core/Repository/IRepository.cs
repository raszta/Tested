using App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace App.Core
{
    public interface IRepository<T> where T : class, IBaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        void Delete(T entity);
        T Add(T entity);
        bool Exist(Expression<Func<T, bool>> predicate);
        void Update(T entity);
        Task<bool> SaveAsync();
    }
}
