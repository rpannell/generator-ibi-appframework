using IBI.<%= Name %>.Application.Utils.Session.Interfaces;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IBI.<%= Name %>.Application.Utils.Session
{
    public static class WebserviceSessionServiceCollectionExtensions
    {
        #region Methods

        public static IServiceCollection AddWebserviceSession(this IServiceCollection services, Action<WebserviceSessionOptions> setupAction)
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
            services.AddSingleton<ISessionService, SessionService>();
            services.AddTransient<ISessionStore, WebserviceSessionStore>();

            return services;
        }

        #endregion Methods
    }
}