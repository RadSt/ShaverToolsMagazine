using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ShaverToolsShop.Conventions.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        T Get(object id);
        T GetIncluding(object id, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetIncludingAsync(object id, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetAsync(object id);
        IQueryable<T> GetAll();
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        T Delete(T entity);
        void Edit(T entity);
        void AddRange(IList<T> entities);
        void DeleteRange(IList<T> entities);
        void Save();
        Task SaveAsync();
    }
}