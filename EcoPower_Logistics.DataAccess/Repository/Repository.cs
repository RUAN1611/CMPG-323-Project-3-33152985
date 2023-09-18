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

        // IncludeEntities allows a controller to include related entities in their methods.
        public async Task<T> Get(Expression<Func<T, bool>>? filter, string? includeEntities = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeEntities))
            {
                foreach (var entity in includeEntities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(entity); // Allows users to include related entities.
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetById(int? id)
        {
            return await dbSet.FindAsync(id);
        }

        // Method used to check if a specific record exists for an entity table.
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

        // IncludeEntities allows a controller to include related entities in their methods.
        public async Task<IEnumerable<T>> GetAll(string? includeEntities = null)
        {
            IQueryable<T> query = dbSet;

            if (!string.IsNullOrEmpty(includeEntities))
            {
                foreach (var entity in includeEntities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(entity); // Allows users to include related entities.
                }
            }

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
