using App.Core;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace App.Data
{
    public class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public T Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(string.Format("entity {0}", typeof(T)));
            }
            _context.Set<T>().Add(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Set<T>().Remove(entity);
        }

        public bool Exist(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate is null");
            }
            var exist = _context.Set<T>().Where(predicate);
            return exist.Any();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var queryable = _context.Set<T>().Where(i => true);
            return await queryable.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            var queryable = _context.Set<T>().Where(it => it.Id == id);

            return await queryable.FirstOrDefaultAsync();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(string.Format("entity is null {0}", typeof(T)));
            }
            _context.Set<T>().Update(entity);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
