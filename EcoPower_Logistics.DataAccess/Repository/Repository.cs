using Data;
using EcoPower_Logistics.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EcoPower_Logistics.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SuperStoreContext _db;
        internal DbSet<T> dbSet;

        public Repository(SuperStoreContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async void Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task<T> Get(Expression<Func<T, bool>>? filter)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetById(int? id)
        {
            return await dbSet.FindAsync(id);
        }

        public bool Exists(Expression<Func<T, bool>>? filter)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if(query.Count() > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            IQueryable<T> query = dbSet;

            return query;
        }

        // Remove methods will not be async for concurrency control management.

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
