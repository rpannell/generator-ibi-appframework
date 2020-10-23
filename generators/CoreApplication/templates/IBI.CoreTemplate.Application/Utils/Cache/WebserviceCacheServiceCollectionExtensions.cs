using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IBI.<%= Name %>.Application.Utils.Cache
{
    public static class WebserviceCacheServiceCollectionExtensions
    {
        #region Methods

        public static IServiceCollection AddDistributedWebserviceCache(this IServiceCollection services, Action<WebserviceCacheOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddOptions();
            services.Configure(setupAction);
            services.Add(ServiceDescriptor.Singleton<IDistributedCache, WebserviceCache>());

            return services;
        }

        #endregion Methods
    }
}