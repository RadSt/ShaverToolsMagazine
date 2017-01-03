using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using ShaverToolsShop.Conventions;
using ShaverToolsShop.Conventions.Mapping;
using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Conventions.Services;
using ShaverToolsShop.Data;

namespace ShaverToolsShop.Initializers.AutofacConfigs
{
    /// <summary>
    /// Регистрация зависимостей в Autofac
    /// </summary>
    public static class AutofacConfig
    {
        public static void RegisterDependencies(this ContainerBuilder builder)
        {
            var assemblies = new Assembly[]
            {
                typeof(ApplicationDbContext).GetTypeInfo().Assembly,
            };

            // Регистрация Initialize классов
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(IInitializable))))
                .AsImplementedInterfaces();

            // регистрация сервисов
            builder.RegisterAssemblyTypes(assemblies)
               .Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(BaseService<>))))
               .AsImplementedInterfaces().InstancePerRequest();

            // регистрация репозиториев
            builder.RegisterAssemblyTypes(assemblies)
               .Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(GenericReadRepository<>))))
               .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(assemblies)
               .Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(GenericRepository<>))))
               .AsImplementedInterfaces().InstancePerRequest();

            // регистрация Data Access
            builder.RegisterType<ApplicationDbContext>().As<DbContext>();
        }

    }
}