using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EcoPower_Logistics.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        /* IMPORTANT WHEN IMPLEMENTING includeEntities: 
         * includeEntities:"T" in Implementation. Do NOT INCLUDE a SPACE. For example, NOT includeEntities: "T" */
        Task<IEnumerable<T>> GetAll(string? includeEntities = null);
        Task<T> Get(Expression<Func<T, bool>>? filter, string? includeEntities = null);
        Task<T> GetById(int? id);
        bool Exists(Expression<Func<T, bool>>? filter);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
