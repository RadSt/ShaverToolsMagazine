using System.Linq;
using System.Threading.Tasks;

namespace ShaverToolsShop.Conventions.Repositories
{
    public interface IReadRepository<T> where T : class, IEntity
    {
        IQueryable<T> GetAll();
        Task<T> GetAsync(object id);
    }
}