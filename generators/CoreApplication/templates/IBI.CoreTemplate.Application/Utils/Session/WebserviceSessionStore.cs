using IBI.<%= Name %>.Application.Utils.Session.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace IBI.<%= Name %>.Application.Utils.Session
{
    public class WebserviceSessionStore : ISessionStore
    {
        #region Fields

        private readonly ILoggerFactory loggerFactory;
        private readonly ISessionService sessionService;
        private readonly WebserviceSessionOptions settings;

        #endregion Fields

        #region Constructors

        public WebserviceSessionStore(ILoggerFactory loggerFactory, ISessionService sessionService, IOptions<WebserviceSessionOptions> settings)
        {
            this.loggerFactory = loggerFactory;
            this.sessionService = sessionService;
            this.settings = settings.Value;
        }

        #endregion Constructors

        #region Methods

        public ISession Create(string sessionKey, TimeSpan idleTimeout, TimeSpan ioTimeout, Func<bool> tryEstablishSession, bool isNewSessionKey)
        {
            if (string.IsNullOrEmpty(sessionKey))
            {
                throw new ArgumentException("ArgumentCannotBeNullOrEmpty", nameof(sessionKey));
            }

            if (tryEstablishSession == null)
            {
                throw new ArgumentNullException(nameof(tryEstablishSession));
            }

            return new WebserviceSession(string.Format("{1}:{0}", sessionKey, this.settings.InstanceName), idleTimeout, ioTimeout, tryEstablishSession, this.loggerFactory, isNewSessionKey, this.sessionService);
        }

        #endregion Methods
    }
}