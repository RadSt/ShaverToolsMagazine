using ShaverToolsShop.Conventions.ServicesAndRepos;

namespace ShaverToolsShop.Conventions.Services
{
    public class BaseService<T> : IService<T> where T : class, IEntity
    {
    }
}