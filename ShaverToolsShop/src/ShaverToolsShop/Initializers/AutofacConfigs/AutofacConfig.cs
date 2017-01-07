using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using ShaverToolsShop.Conventions;
using ShaverToolsShop.Data;

namespace ShaverToolsShop.Initializers.AutofacConfigs
{
    /// <summary>
    ///     Регистрация зависимостей в Autofac
    /// </summary>
    public static class AutofacConfig
    {
        public static void RegisterDependencies(this ContainerBuilder builder)
        {
            var assemblies = new[]
            {
                typeof(ApplicationDbContext).GetTypeInfo().Assembly
            };

            // Регистрация Initialize классов
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(IInitializable))))
                .AsImplementedInterfaces();

            // регистрация сервисов
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            // регистрация репозиториев
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            // регистрация Data Access
            builder.RegisterType<ApplicationDbContext>().As<DbContext>();
        }
    }
}