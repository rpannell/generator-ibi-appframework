
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace IBI.<%= Name %>.Application.Utils.Session
{
    /// <summary>
    /// An <see cref="ISessionStore"/> backed by an <see cref="IDistributedCache"/>.
    /// </summary>
    public class RedisSessionStore : ISessionStore
    {
        private readonly IDistributedCache _cache;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="DistributedSessionStore"/>. 
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
        /// <param name="configuration"></param>
        public RedisSessionStore(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

             _cache = new RedisCache(new RedisCacheOptions()
            {
                Configuration = configuration["Redis:SessionStore"],
                InstanceName = "<%= Name %>"
            }); ;
            _loggerFactory = loggerFactory;
        }

        /// <inheritdoc />
        public ISession Create(string sessionKey, TimeSpan idleTimeout, TimeSpan ioTimeout, Func<bool> tryEstablishSession, bool isNewSessionKey)
        {
            if (string.IsNullOrEmpty(sessionKey))
            {
                throw new ArgumentException("Cannot be null", nameof(sessionKey));
            }

            if (tryEstablishSession == null)
            {
                throw new ArgumentNullException(nameof(tryEstablishSession));
            }

            return new DistributedSession(_cache, sessionKey, idleTimeout, ioTimeout, tryEstablishSession, _loggerFactory, isNewSessionKey);
        }
    }
}
