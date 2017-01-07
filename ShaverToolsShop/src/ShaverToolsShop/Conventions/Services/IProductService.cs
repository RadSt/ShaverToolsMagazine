using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShaverToolsShop.Conventions.ServicesAndRepos;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Conventions.Services
{
    public interface IProductService: IService<Product>
    {
        Task<IEnumerable<SelectListItem>> GetAllForSelect();
    }
}