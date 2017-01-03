using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Conventions.Repositories
{
    public class GenericReadRepository<T> : IReadRepository<T>
       where T : BaseEntity
    {
        private readonly DbSet<T> _dbset;

        public GenericReadRepository(DbContext context)
        {
            _dbset = context.Set<T>();
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbset.AsNoTracking();
        }

        public Task<T> GetAsync(object id)
        {
            return _dbset.AsNoTracking().FirstOrDefaultAsync(x => x.Id == (Guid)id);
        }
    }
}