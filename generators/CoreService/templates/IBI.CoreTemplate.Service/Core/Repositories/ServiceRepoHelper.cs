using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IBI.<%= Name %>.Service.Core.Repositories
{
    /// <summary>
    /// Used to hand IServiceCollection and adding the Repositories
    /// to the service collection via a generic call with a type
    /// </summary>
    public static class ServiceRepositoryInstaller
    {
        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddAllDbContext(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            try
            {
                var currentRepositories = Assembly.GetEntryAssembly().GetTypes()
                                            .Where(x => x.GetBaseClassesAndInterfaces().Contains(typeof(DbContext)))
                                            .ToList();
                if (currentRepositories != null)
                {
                    foreach (var repo in currentRepositories)
                    {
                        return serviceCollection.AddDbContext(repo, optionsAction, contextLifetime);
                    }
                }
            }
            catch (Exception) { }
            return serviceCollection;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="repositoryType"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection, Type repositoryType, Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            try
            {
                var method = typeof(EntityFrameworkServiceCollectionExtensions)
                            .GetMethod("AddDbContext", new Type[] { typeof(IServiceCollection), typeof(Action<DbContextOptionsBuilder>), typeof(ServiceLifetime) })
                            .MakeGenericMethod(new Type[] { repositoryType })
                            .Invoke(serviceCollection, new object[] { serviceCollection, optionsAction, contextLifetime });
            }
            catch (Exception) { }
            return serviceCollection;
        }

        /// <summary>
        /// Add the dbcontext for all of the repositories
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="repositoryType"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection, Type repositoryType, ServiceLifetime contextLifetime)
        {
            try
            {
                var method = typeof(EntityFrameworkServiceCollectionExtensions)
                        .GetMethod("AddDbContext", new Type[] { typeof(IServiceCollection), typeof(ServiceLifetime) })
                        .MakeGenericMethod(new Type[] { repositoryType })
                        .Invoke(serviceCollection, new object[] { serviceCollection, contextLifetime });
            }
            catch (Exception) { }
            return serviceCollection;
        }

        /// <summary>
        /// Add the dbcontext for all of the repositories
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="repositoryType"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection, Type repositoryType, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            try
            {
                var method = typeof(EntityFrameworkServiceCollectionExtensions)
                        .GetMethod("AddDbContext", new Type[] { typeof(IServiceCollection), typeof(Action<IServiceProvider, DbContextOptionsBuilder>), typeof(ServiceLifetime) })
                        .MakeGenericMethod(new Type[] { repositoryType })
                        .Invoke(serviceCollection, new object[] { serviceCollection, optionsAction, contextLifetime });
            }
            catch (Exception) { }
            return serviceCollection;
        }

        /// <summary>
        /// Returns the base class and interfaces based on a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Type> GetBaseClassesAndInterfaces(this Type type)
        {
            return type.GetTypeInfo().BaseType == null || type.GetTypeInfo().BaseType == typeof(object)
                    ? type.GetInterfaces().ToList()
                    : Enumerable.Repeat(type.GetTypeInfo().BaseType, 1)
                                .Concat(type.GetInterfaces())
                                .Concat(type.GetTypeInfo().BaseType.GetBaseClassesAndInterfaces())
                                .Distinct()
                                .ToList();
        }

        #endregion Methods
    }
}