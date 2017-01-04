using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Conventions.Repositories
{
    public class GenericRepository<T> : IRepository<T>
       where T : BaseEntity
    {
        private readonly DbContext _entities;
        private readonly DbSet<T> _dbset;


        public GenericRepository(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<T>();
        }

        public T Get(object id)
        {
            var entity = _dbset.Find(id);

            return entity;
        }

        public Task<T> GetAsync(object id)
        {
            var entity = _dbset.FindAsync((Guid)id);

            return entity;
        }

        public T GetIncluding(object id, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbset.AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            var entity = query.FirstOrDefault(x => x.Id == (Guid)id);

            return entity;
        }

        public Task<T> GetIncludingAsync(object id, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbset.AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            var entity = query.FirstOrDefaultAsync(x => x.Id == (Guid)id);

            return entity;
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbset;
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {

            IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
            return query;
        }

        public virtual T Add(T entity)
        {
            entity.Id = Guid.NewGuid();
            var res = _dbset.Add(entity);

            return res.Entity;
        }

        public virtual T Delete(T entity)
        {
            return _dbset.Remove(entity).Entity;
        }

        public void Edit(T entity)
        {
        }

        public void AddRange(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.Id = Guid.NewGuid();
            }

            _dbset.AddRange(entities);
        }

        public void DeleteRange(IList<T> entities)
        {
            _dbset.RemoveRange(entities);
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }

        public virtual Task SaveAsync()
        {
            return _entities.SaveChangesAsync();
        }
    }
}